using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoInfo;
using System.IO;
using System.Diagnostics;
namespace ReplayParser
{
    class Program
    {
        static void Main(string[] args)
        {
            bool headersWritten = false;
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] replays = Directory.GetFiles(folder, "*.dem");
            int progress = 0;
            foreach (string replay in replays)
            {
                progress++;
                using (var file = File.OpenRead(replay))
                {
                    Console.WriteLine("Opening demo " + progress);
                    using (var parser = new DemoParser(file))
                    {
                        parser.ParseHeader();
                        string map = parser.Map, outputPos = progress + "_" + map + "_pos" +".csv", outputMeta = progress + "_" + map + "_meta" + ".csv";
                        var outputStreamPos = new StreamWriter(outputPos);
                        var outputStreamMeta = new StreamWriter(outputMeta);
                        int ctStartroundMoney = 0, tStartroundMoney = 0, ctEquipValue = 0, tEquipValue = 0, ctSaveAmount = 0, tSaveAmount = 0;
                        Dictionary<Player, int> killsThisRound = new Dictionary<Player, int>();
                        bool hasMatchStarted = false;
                        int defuses = 0;
                        int plants = 0;
                        int tickTmr = 0;
                        parser.MatchStarted += (sender, e) => {
                            hasMatchStarted = true;
                        };
                        
                        if(!headersWritten)
                        {
                            writeMetaHeaders(outputStreamMeta);
                            writePosHeaders(outputStreamPos);
                            headersWritten = true;
                        }

                        parser.PlayerKilled += (object sender, PlayerKilledEventArgs e) => {
                            if (e.Killer != null)
                            {
                                if (!killsThisRound.ContainsKey(e.Killer))
                                    killsThisRound[e.Killer] = 0;
                                killsThisRound[e.Killer]++;
                            }
                        };

                        parser.RoundStart += (sender, e) => {
                            if (!hasMatchStarted)
                                return;
                            ctStartroundMoney = parser.Participants.Where(a => a.Team == Team.CounterTerrorist).Sum(a => a.Money);
                            tStartroundMoney = parser.Participants.Where(a => a.Team == Team.Terrorist).Sum(a => a.Money);
                            ctSaveAmount = parser.Participants.Where(a => a.Team == Team.CounterTerrorist && a.IsAlive).Sum(a => a.CurrentEquipmentValue);
                            tSaveAmount = parser.Participants.Where(a => a.Team == Team.Terrorist && a.IsAlive).Sum(a => a.CurrentEquipmentValue);
                            plants = 0; defuses = 0;
                            killsThisRound.Clear();
                        };

                        parser.FreezetimeEnded += (sender, e) => {
                            if (!hasMatchStarted)
                                return;
                            ctEquipValue = parser.Participants.Where(a => a.Team == Team.CounterTerrorist).Sum(a => a.CurrentEquipmentValue);
                            tEquipValue = parser.Participants.Where(a => a.Team == Team.Terrorist).Sum(a => a.CurrentEquipmentValue);
                        };

                        parser.BombPlanted += (sender, e) => {
                            if (!hasMatchStarted)
                                return;
                            plants++;
                        };

                        parser.BombDefused += (sender, e) => {
                            if (!hasMatchStarted)
                                return;
                            defuses++;
                        };

                        parser.TickDone += (sender, e) => {
                            if (!hasMatchStarted)
                                return;
                            if (tickTmr == 32)
                            {
                                tickTmr = 0;
                                int playerid = 0;
                                foreach (var player in parser.PlayingParticipants)
                                {
                                    playerid++;
                                    if(player.IsAlive == true)
                                    {
                                        PrintPos(parser, outputStreamPos, (int)player.Position.X, (int)player.Position.Y, (int)player.ViewDirectionX, (int)player.ViewDirectionY, playerid, player.Team.ToString());
                                    }
                                    //float currentWay = (float)(player.Velocity.Absolute * parser.TickTime);
                                    //if (player.Team == Team.CounterTerrorist)
                                    //ctWay += currentWay;
                                    //else if (player.Team == Team.Terrorist)
                                    //tWay += currentWay;
                                }
                                return;
                            }
                            tickTmr++;
                        };

                        parser.RoundEnd += (sender, e) => {
                            if (!hasMatchStarted)
                                return;
                            PrintRoundResults(parser, outputStreamMeta, ctStartroundMoney, tStartroundMoney, ctEquipValue, tEquipValue, ctSaveAmount, tSaveAmount, defuses, plants, killsThisRound);
                        };

                        parser.ParseToEnd();
                        PrintRoundResults(parser, outputStreamMeta, ctStartroundMoney, tStartroundMoney, ctEquipValue, tEquipValue, ctSaveAmount, tSaveAmount, defuses, plants, killsThisRound);
                        outputStreamMeta.Close();
                        outputStreamPos.Close();
                    }
                }
            }
        }
        static void writeMetaHeaders(StreamWriter stream)
        {
            stream.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18}", "ROUND", "CTSCORE", "TSCORE", "CTALIVE", "TALIVE", "CTSTARTROUNDMONEY", "TSTARTROUNDMONEY", "CTEQUIPVALUE", "TEQUIPVALUE", "CTSAVEAMOUNT", "TSAVEAMOUNT", "CTKILLS","TKILLS","CTDEATHS","TDEATHS","CTASSISTS","TASSISTS","PLANTS","DEFUSES"));
        }
        static void writePosHeaders(StreamWriter stream)
        {
            stream.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", "ROUND", "X", "Y", "VIEWX", "VIEWY", "PLAYERID","TEAM"));
        }
        static void PrintRoundResults(DemoParser parser, StreamWriter outputStream, int ctStartroundMoney, int tStartroundMoney, int ctEquipValue, int tEquipValue, int ctSaveAmount, int tSaveAmount, int defuses, int plants, Dictionary<Player, int> killsThisRound)
        {

            //At the end of each round, let's write down some statistics!
            outputStream.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18}", parser.CTScore + parser.TScore, //Round-Number
                parser.CTScore, parser.TScore, //how many CTs are still alive?
                parser.PlayingParticipants.Count(a => a.IsAlive && a.Team == Team.CounterTerrorist), //how many Ts are still alive?
                parser.PlayingParticipants.Count(a => a.IsAlive && a.Team == Team.Terrorist), ctStartroundMoney, tStartroundMoney, ctEquipValue, tEquipValue, ctSaveAmount, tSaveAmount, //The kills of all CTs so far
                parser.PlayingParticipants.Where(a => a.Team == Team.CounterTerrorist).Sum(a => a.AdditionaInformations.Kills), parser.PlayingParticipants.Where(a => a.Team == Team.Terrorist).Sum(a => a.AdditionaInformations.Kills), //The deaths of all CTs so far
                parser.PlayingParticipants.Where(a => a.Team == Team.CounterTerrorist).Sum(a => a.AdditionaInformations.Deaths), parser.PlayingParticipants.Where(a => a.Team == Team.Terrorist).Sum(a => a.AdditionaInformations.Deaths), //The assists of all CTs so far
                parser.PlayingParticipants.Where(a => a.Team == Team.CounterTerrorist).Sum(a => a.AdditionaInformations.Assists), parser.PlayingParticipants.Where(a => a.Team == Team.Terrorist).Sum(a => a.AdditionaInformations.Assists), plants, defuses //The name of the topfragger this round

            ));
        }

        static void PrintPos(DemoParser parser, StreamWriter outputStream, int x, int y, int viewx, int viewy, int playerid, string team)
        {
            int teamid = 0;
            if (team == "Terrorist") teamid = 1;
            outputStream.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", parser.CTScore + parser.TScore, x, y, viewx, viewy, playerid, teamid));
        }
    }
}

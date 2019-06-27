using Grapevine.Server;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AuthService
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                var v = "v0.1.0.0";
                var nameService = "Auth Service";
                Console.Title = nameService + " " + v;
                Console.WriteLine("     A U T H   S E R V I C E    " + v);
                var serverStandar = new RestServer();

                var input = "";
                while ((input = Console.ReadLine()) != "q")
                {
                    switch (input)
                    {
                        case "start":
                            Console.WriteLine("Starting service...");
                            serverStandar.Port = "5011";
                            serverStandar.Host = "*";
                            serverStandar.Start();
                            Console.Title = "[ON]  " + nameService + " " + v;

                            break;


                        case "start --log":
                            Console.WriteLine("Starting service...");

                            using (var server = new RestServer())
                            {
                                server.Port = "5011";
                                server.Host = "*";
                                server.LogToConsole().Start();
                                Console.Title = "[ON]  " + nameService + " " + v;
                                Console.ReadLine();
                                server.Stop();
                            }

                            break;
                        case "stop":
                            Console.WriteLine("Stopping service...");
                            serverStandar.Stop();
                            Console.Title = nameService + " " + v;
                            break;
                        case "--version":
                            Console.WriteLine(v);

                            break;
                        default:
                            Console.WriteLine(String.Format("Unknown command: {0}", input));
                            break;
                    }

                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Camera_Bot___Client
{
    /// <summary>
    /// The settings used to represent a user's settings.
    /// </summary>
    public class Settings
    {
        #region Limiters

        /// <summary>
        /// The regex for an IPv4 address.
        /// </summary>
        public static readonly Regex IPv4_regex = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", 
            RegexOptions.Compiled);


        /// <summary>
        /// The minimum number a port can be, inclusive.
        /// </summary>
        public static int MinPort => 1024;

        /// <summary>
        /// The maximum number a port can be, inclusive.
        /// </summary>
        public static int MaxPort => 49151;

        #endregion Limiters


        #region Properties

        /// <summary>
        /// The IP address of the server to connect to.
        /// </summary>
        public IPAddress IPAddress { get; set; }


        /// <summary>
        /// The port of the server to connect to.
        /// </summary>
        public int Port { get; set; }




        /// <summary>
        /// The key that will move the camera up.
        /// </summary>
        public Key Up { get; set; }

        /// <summary>
        /// The key that will move the camera down.
        /// </summary>
        public Key Down { get; set; }

        /// <summary>
        /// The key that will move the camera left.
        /// </summary>
        public Key Left { get; set; }

        /// <summary>
        /// The key that will move the camera right.
        /// </summary>
        public Key Right { get; set; }

        #endregion Properties


        /// <summary>
        /// Saves the current settings to a file.
        /// </summary>
        /// <param name="path">The path of the file to save to.</param>
        public void SaveToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(IPAddress.ToString());
                writer.WriteLine(Port);

                writer.WriteLine(Up.ToString());
                writer.WriteLine(Down.ToString());
                writer.WriteLine(Left.ToString());
                writer.WriteLine(Right.ToString());
            }
        }


        #region ReadFromFile

        /// <summary>
        /// Creates a Settings object from the given file.
        /// </summary>
        /// <param name="path">The path of the file to read from.</param>
        /// <returns>The created Settings object.</returns>
        /// <exception cref="System.FormatException">When the settings file is malformed.</exception>
        public static Settings FromFile(string path)
        {
            Settings created = new Settings();

            created.ReadFromFile(path);

            return created;
        }


        /// <summary>
        /// Reads the file at the given path and parses the settings out of it into this object.
        /// </summary>
        /// <param name="path">The path of the file to read from.</param>
        /// <exception cref="System.FormatException">When the settings file is malformed.</exception>
        public void ReadFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                //IPAddress:
                IPAddress = IPv4_FromString(reader.ReadLine());

                if (IPAddress == null)
                {
                    throw new FormatException("The settings file is malformed.");
                }


                //Port:
                int temp;
                if (!int.TryParse(reader.ReadLine(), out temp))
                {
                    throw new FormatException("The settings file is malformed.");
                }
                else
                {
                    if (temp < MinPort || temp > MaxPort)
                    {
                        throw new FormatException("The settings file is malformed.");
                    }
                }

                Port = temp;


                //Keys:
                Key temp_key;

                if (!Enum.TryParse<Key>(reader.ReadLine(), out temp_key))
                {
                    throw new FormatException("The settings file is malformed.");
                }
                Up = temp_key;


                if (!Enum.TryParse<Key>(reader.ReadLine(), out temp_key))
                {
                    throw new FormatException("The settings file is malformed.");
                }
                Down = temp_key;


                if (!Enum.TryParse<Key>(reader.ReadLine(), out temp_key))
                {
                    throw new FormatException("The settings file is malformed.");
                }
                Left = temp_key;


                if (!Enum.TryParse<Key>(reader.ReadLine(), out temp_key))
                {
                    throw new FormatException("The settings file is malformed.");
                }
                Right = temp_key;
            }
        }

        #endregion ReadFromFile


        #region Helper Methods

        /// <summary>
        /// Converts a string into an IPv4 address.
        /// </summary>
        /// <param name="address">The string containing only the address.</param>
        /// <returns>The IPAddress object that represents the given IPv4 address. Null if malformatted.</returns>
        public static IPAddress IPv4_FromString(string address)
        {
            if (!IPv4_regex.IsMatch(address))
            {
                return null;
            }


            string[] splits = address.Split('.');

            return new IPAddress(new byte[] { byte.Parse(splits[0]), byte.Parse(splits[1]), byte.Parse(splits[2]), byte.Parse(splits[3]) });
        }

        #endregion Helper Methods
    }
}

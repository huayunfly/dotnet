using System;
using System.Collections.Generic;
using System.Text;

namespace com.huayunfly.app
{
    public class Racer : object, IComparable<Racer>, IFormattable
    {
        public Racer(string firstName, string lastName, string country,
           int starts, int wins) : this(firstName, lastName, country, starts, wins, null, null) { }
        public Racer(string firstName, string lastName, string country,
            int starts, int wins, IEnumerable<int> years, IEnumerable<string> cars)
        {
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            Starts = starts;
            Wins = wins;
            Years = years != null ? new List<int>(years) : new List<int>();
            Cars = cars != null ? new List<string>(cars) : new List<string>();
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Country { get; }
        public int Starts { get; }
        public int Ends { get;}
        public int Wins { get; }
        public IEnumerable<string> Cars { get; }
        public IEnumerable<int> Years { get; }

        public int CompareTo(Racer other) => LastName.CompareTo(other?.LastName);
      
        public override string ToString() => $"{LastName} {FirstName}";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case null:
                case "N":
                    return ToString();
                case "L":
                    return LastName;
                case "F":
                    return FirstName;
                case "C":
                    return Country;
                case "S":
                    return Starts.ToString();
                case "W":
                    return Wins.ToString();
                case "A":
                    return $"{FirstName} {LastName}, {Country}; starts: {Starts}, wins: {Wins}";
                default:
                    throw new FormatException($"Format {format} not supported");            
            }
            
        }
    }
}

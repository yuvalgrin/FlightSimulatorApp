using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Models
{
    public static class FlightSimulatorResources
    {
        public static readonly Dictionary<string, string> ShortNameToFull = new Dictionary<string, string>()
            {
                {"AirSpeed", "/instrumentation/airspeed-indicator/indicated-speed-kt"},
                {"Altimeter", "/instrumentation/altimeter/indicated-altitude-ft"},
                {"Pitch", "/instrumentation/attitude-indicator/internal-pitch-deg"},
                {"Roll", "/instrumentation/attitude-indicator/internal-roll-deg"},
                {"Altitude", "/instrumentation/gps/indicated-altitude-ft"},
                {"GroundSpeed", "/instrumentation/gps/indicated-ground-speed-kt"},
                {"VerticalSpeed", "/instrumentation/gps/indicated-vertical-speed"},
                {"Heading", "/instrumentation/heading-indicator/indicated-heading-deg"},
                {"Aileron", "/controls/flight/aileron"},
                {"Elevator", "/controls/flight/elevator"},
                {"Rudder", "/controls/flight/rudder"},
                {"Throttle", "/controls/engines/current-engine/throttle"},
                {"Latitude", "/position/latitude-deg"},
                {"Longitude", "/position/longitude-deg"}
            };
        public static readonly Dictionary<string, string> FullNameToShort = new Dictionary<string, string>()
            {
                {"/instrumentation/airspeed-indicator/indicated-speed-kt", "AirSpeed"},
                {"/instrumentation/altimeter/indicated-altitude-ft","Altimeter"},
                {"/instrumentation/attitude-indicator/internal-pitch-deg", "Pitch"},
                {"/instrumentation/attitude-indicator/internal-roll-deg", "Roll"},
                {"/instrumentation/gps/indicated-altitude-ft","Altitude"},
                {"/instrumentation/gps/indicated-ground-speed-kt", "GroundSpeed"},
                {"/instrumentation/gps/indicated-vertical-speed", "VerticalSpeed"},
                {"/instrumentation/heading-indicator/indicated-heading-deg", "Heading"},
                {"/controls/flight/aileron", "Aileron"},
                {"/controls/flight/elevator","Elevator"},
                {"/controls/flight/rudder", "Rudder"},
                {"/controls/engines/current-engine/throttle", "Throttle"},
                {"/position/latitude-deg", "Latitude"},
                {"/position/longitude-deg", "Longitude"}
            };
    }
}

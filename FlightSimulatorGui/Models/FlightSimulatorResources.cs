using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorGui.Models
{
    public class FlightSimulatorResources
    {
        public static Dictionary<string, string> shortNameToFull = new Dictionary<string, string>()
            {
                {"air_speed", "/instrumentation/airspeed-indicator/indicated-speed-kt"},
                {"altimeter", "/instrumentation/altimeter/indicated-altitude-ft"},
                {"pitch", "/instrumentation/attitude-indicator/internal-pitch-deg"},
                {"roll", "/instrumentation/attitude-indicator/internal-roll-deg"},
                {"altitude", "/instrumentation/gps/indicated-altitude-ft"},
                {"ground_speed", "/instrumentation/gps/indicated-ground-speed-kt"},
                {"vertical_speed", "/instrumentation/gps/indicated-vertical-speed"},
                {"heading", "/instrumentation/heading-indicator/indicated-heading-deg"},
                {"aileron", "/controls/flight/aileron"},
                {"elevator", "/controls/flight/elevator"},
                {"rudder", "/controls/flight/rudder"},
                {"throttle", "/controls/engines/current-engine/throttle"},
                {"latitude", "/position/latitude-deg"},
                {"longitude", "/position/longitude-deg"}
            };
        public static Dictionary<string, string> fullNameToShort = new Dictionary<string, string>()
            {
                {"/instrumentation/airspeed-indicator/indicated-speed-kt", "air_speed"},
                {"/instrumentation/altimeter/indicated-altitude-ft","altimeter"},
                {"/instrumentation/attitude-indicator/internal-pitch-deg", "pitch"},
                {"/instrumentation/attitude-indicator/internal-roll-deg", "roll"},
                {"/instrumentation/gps/indicated-altitude-ft","altitude"},
                {"/instrumentation/gps/indicated-ground-speed-kt", "ground_speed"},
                {"/instrumentation/gps/indicated-vertical-speed", "vertical_speed"},
                {"/instrumentation/heading-indicator/indicated-heading-deg", "heading"},
                {"/controls/flight/aileron", "aileron"},
                {"/controls/flight/elevator","elevator"},
                {"/controls/flight/rudder", "rudder"},
                {"/controls/engines/current-engine/throttle", "throttle"},
                {"/position/latitude-deg", "latitude"},
                {"/position/longitude-deg", "longitude"}
            };
    }
}

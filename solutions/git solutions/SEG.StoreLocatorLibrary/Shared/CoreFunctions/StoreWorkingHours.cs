using System;
using System.Collections.Generic;

namespace SEG.StoreLocatorLibrary.Shared.CoreFunctions
{
    public class StoreWorkingHours
    {
        private IDictionary<string, string> _workingHours = new Dictionary<string, string>();

        public string GetFormattedHours(IList<SimTimings> timings)
        {
            foreach (var timing in timings)
            {
                var key = timing.Day[..3].ToUpper();
                _workingHours.Add(key, ParseHours(timing));
            }

            SetWeekDays();

            return $"{_workingHours["WEEKDAYS"]}{_workingHours["FRI"]}, SAT: {_workingHours["SAT"]}, SUN: {_workingHours["SUN"]}";
        }

        public string ParseHours(SimTimings timing)
        {
            if (string.IsNullOrEmpty(timing.StoreOpenTime) || string.IsNullOrEmpty(timing.StoreCloseTime))
                return "";

            try
            {
                var openTime = Convert.ToDateTime(timing.StoreOpenTime).ToString("h:mm tt");
                var closeTime = Convert.ToDateTime(timing.StoreCloseTime).ToString("h:mm tt");
                return $"{openTime} - {closeTime}";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private void SetWeekDays()
        {
            if (_workingHours["THU"] == _workingHours["FRI"])
            {
                _workingHours.Add("WEEKDAYS", $"MON - FRI: {_workingHours["THU"]}");
                _workingHours["FRI"] = "";
            }
            else
            {
                _workingHours.Add("WEEKDAYS", $"MON - THU: {_workingHours["THU"]}");
                _workingHours["FRI"] = $", FRI: {_workingHours["FRI"]}";
            }
        }

    }
}

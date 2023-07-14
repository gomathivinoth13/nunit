using System;
using System.Collections.Generic;

namespace SEG.StoreLocatorLibrary.Shared.CoreFunctions
{
    public class PharmacyWorkingHours
    {
        private IDictionary<string, string> _workingHours = new Dictionary<string, string>();

        public string GetFormattedHours(SimPharmacy pharmacy)
        {
            if (string.IsNullOrEmpty(pharmacy.PharmacyHoursMon) &&
                string.IsNullOrEmpty(pharmacy.PharmacyHoursTue) &&
                string.IsNullOrEmpty(pharmacy.PharmacyHoursWed) &&
                string.IsNullOrEmpty(pharmacy.PharmacyHoursThu) &&
                string.IsNullOrEmpty(pharmacy.PharmacyHoursFri) &&
                string.IsNullOrEmpty(pharmacy.PharmacyHoursSat) &&
                string.IsNullOrEmpty(pharmacy.PharmacyHoursSun) &&
                string.IsNullOrEmpty(pharmacy.PharmacyPhone))
                return null;

            _workingHours.Add("MON", ParseHours(pharmacy.PharmacyHoursMon));
            _workingHours.Add("TUE", ParseHours(pharmacy.PharmacyHoursTue));
            _workingHours.Add("WED", ParseHours(pharmacy.PharmacyHoursWed));
            _workingHours.Add("THU", ParseHours(pharmacy.PharmacyHoursThu));
            _workingHours.Add("FRI", ParseHours(pharmacy.PharmacyHoursFri));
            _workingHours.Add("SAT", ParseHours(pharmacy.PharmacyHoursSat));
            _workingHours.Add("SUN", ParseHours(pharmacy.PharmacyHoursSun));

            SetWeekDays();

            return $"{_workingHours["WEEKDAYS"]}{_workingHours["FRI"]}, SAT: {_workingHours["SAT"]}, SUN: {_workingHours["SUN"]}";
        }

        public string ParseHours(string hours)
        {
            if (string.IsNullOrEmpty(hours)) return "";

            var pharmacyTime = hours.Split('-');

            if (pharmacyTime.Length == 2)
            {
                var openTime = Convert.ToDateTime(pharmacyTime[0]).ToString("h:mm tt");
                var closeTime = Convert.ToDateTime(pharmacyTime[1]).ToString("h:mm tt");
                return $"{openTime} - {closeTime}";
            }
            return "";
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

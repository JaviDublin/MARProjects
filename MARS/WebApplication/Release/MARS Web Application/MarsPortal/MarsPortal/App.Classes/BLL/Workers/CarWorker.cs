using System;
using System.Data; // added for datatable

namespace App.BLL.Workers {

    public class CarWorker {
        // created on the 24-7-2012 to condition the data return from the stored procedure
        // spReportFleetComparisonCarClass and spReportFleetComparisonCarSegment called from the using class (AvailabilityFleetComparison)

        private string _topic;
        private string _divisor;

        public CarWorker(string topic) {
            if(topic.Equals("UTILISATION"))
                _topic = "on_rent";
            else if(topic.Contains("UTILISATION") && topic.Contains("Overdue"))
                _topic = ("on_rent&overdue");
            else
                _topic = topic.Replace(' ', '_').ToLower();
            if(_topic.Contains("operational_fleet")) _divisor = "TOTAL_FLEET";
            else _divisor="OPERATIONAL_FLEET";
        }
        public DataTable getDataCarSegment(string fieldName, DataTable dt, DateTime startDate, DateTime endDate, bool percentage) {

            DataTable returnDt = new DataTable("Fleet comparison " + fieldName);

            if(dt.Rows.Count > 0) {
                returnDt.Columns.Add("car_group");
                returnDt.Columns.Add("car_count");
                var q = from p in dt.AsEnumerable() select p;
                double noOfDays = endDate.Subtract(startDate).Days;
                noOfDays = noOfDays < 1 ?1:noOfDays = 1;
                double val = 0;
                foreach(var item in q) {
                    if(_topic.Equals("on_rent&overdue"))
                        val = Convert.ToDouble(item["on_rent"]) + Convert.ToInt32(item["overdue"]);
                    else
                        val = Convert.ToDouble(item[_topic]);

                    if(percentage) val = val / Convert.ToDouble(item[_divisor]) * 100;

                    returnDt.Rows.Add(item[fieldName], val);
                }
            }
            return returnDt;
        }

        public DataTable getDataCarClass(string fieldName, DataTable dt, DateTime startDate, DateTime endDate, bool percentage) {

            DataTable returnDt = new DataTable("Fleet comparison " + fieldName);

            returnDt.Columns.Add("car_group");
            returnDt.Columns.Add("car_count");
            var q = from p in dt.AsEnumerable() select p;
            double noOfDays = endDate.Subtract(startDate).Days;
            noOfDays = noOfDays < 1 ?1:noOfDays = 1;
            double val = 0;
            if(percentage) {
                foreach(var item in q) {
                    if(_topic.Equals("on_rent&overdue"))
                        val = 100 * (((Convert.ToInt32(item["on_rent"]) + Convert.ToInt32(item["overdue"])) / noOfDays)
                                / ((Convert.ToInt32(item[_divisor])) / noOfDays));
                    else
                        val = 100 * ((Convert.ToInt32(item[_topic]) / noOfDays)
                             / ((Convert.ToInt32(item[_divisor])) / noOfDays));
                    returnDt.Rows.Add(item[fieldName], val);
                }

            } else {
                foreach(var item in q) {
                    if(_topic.Equals("on_rent&overdue"))
                        val = ((Convert.ToInt32(item["on_rent"]) + Convert.ToInt32(item["overdue"])) / noOfDays);
                    else
                        val = (Convert.ToInt32(item[_topic]) / noOfDays);
                    returnDt.Rows.Add(item[fieldName], val);
                }
            }
            return returnDt;
        }
    }
}
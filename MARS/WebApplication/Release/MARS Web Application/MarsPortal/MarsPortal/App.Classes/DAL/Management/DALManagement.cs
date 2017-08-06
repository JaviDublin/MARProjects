using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using App.BLL.Utilities;
using App.DAL.Data;
using App.Entities;

namespace App.DAL.Management {
    public class DALManagement {
        public FleetPlanDetailListContainer GetFleetPlanDetailBy(string country,
        int locationGroup,
        int carClassGroup,
        DateTime startDate,
        DateTime endDate) {
            var fleetPlanDetailListContainer = new FleetPlanDetailListContainer();
            var con = DBManager.CreateConnection();
            var cmd = DBManager.CreateProcedure(StoredProcedures.GetFleetPlanDetailBy, con);

            cmd.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.Country].Value = country;
            cmd.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            cmd.Parameters.Add("@locationGroup", SqlDbType.Int);
            cmd.Parameters["@locationGroup"].Value = Convert.ToInt32(locationGroup);
            cmd.Parameters["@locationGroup"].Direction = ParameterDirection.Input;

            cmd.Parameters.Add("@CarClassGroup", SqlDbType.Int);
            cmd.Parameters["@CarClassGroup"].Value = carClassGroup;
            cmd.Parameters["@CarClassGroup"].Direction = ParameterDirection.Input;

            cmd.Parameters.Add("@startDate", SqlDbType.Date);
            cmd.Parameters["@startDate"].Value = startDate;
            cmd.Parameters["@startDate"].Direction = ParameterDirection.Input;

            cmd.Parameters.Add("@endDate", SqlDbType.Date);
            cmd.Parameters["@endDate"].Value = endDate;
            cmd.Parameters["@endDate"].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var fleetPlanEntry = new FleetPlanEntry();

                    if (reader["FleetPlanID"] != DBNull.Value)
                        fleetPlanEntry.FleetPlanID = Convert.ToInt32(reader["FleetPlanID"]);

                    if (reader["ScenarioID"] != DBNull.Value)
                        fleetPlanEntry.ScenarioID = Convert.ToInt32(reader["ScenarioID"]);

                    fleetPlanDetailListContainer.FleetPlanEntryList.Add(fleetPlanEntry);
                }

                reader.NextResult();

                while (reader.Read()) {
                    var fleetPlanDetail = new FleetPlanDetail();

                    if (reader["fleetPlanEntryID"] != DBNull.Value)
                        fleetPlanDetail.FleetPlanEntryID = Convert.ToInt32(reader["fleetPlanEntryID"]);

                    if (reader["fleetPlanDetailID"] != DBNull.Value)
                        fleetPlanDetail.FleetPlanDetailID = Convert.ToInt32(reader["fleetPlanDetailID"]);

                    if (reader["scenarioID"] != DBNull.Value)
                        fleetPlanDetail.ScenarioID = Convert.ToInt32(reader["scenarioID"]);

                    if (reader["targetDate"] != DBNull.Value)
                        fleetPlanDetail.DateOfMovement = DateTime.Parse(reader["targetDate"].ToString());

                    if (reader["car_class_id"] != DBNull.Value)
                        fleetPlanDetail.CarGroup.CarGroupID = Convert.ToInt32(reader["car_class_id"]);

                    if (reader["car_group"] != DBNull.Value)
                        fleetPlanDetail.CarGroup.CarGroupDescription = reader["car_group"].ToString();

                    fleetPlanDetail.LocationGroup = new LocationGroup();

                    if (reader["locationGroupID"] != DBNull.Value)
                        fleetPlanDetail.LocationGroup.LocationGroupID = Convert.ToInt32(reader["locationGroupID"]);

                    if (reader["locationGroupName"] != DBNull.Value)
                        fleetPlanDetail.LocationGroup.LocationGroupName = reader["locationGroupName"].ToString();

                    if (reader["addition"] != DBNull.Value)
                        fleetPlanDetail.Addition = Convert.ToInt32(reader["addition"]);

                    if (reader["deletion"] != DBNull.Value)
                        fleetPlanDetail.Deletion = Convert.ToInt32(reader["deletion"]);

                    if (reader["amount"] != DBNull.Value)
                        fleetPlanDetail.Amount = Convert.ToInt32(reader["amount"]);

                    fleetPlanDetailListContainer.FleetPlanDetailList.Add(fleetPlanDetail);
                }
            }

            return fleetPlanDetailListContainer;
        }

        public FleetPlanDetailListContainer GetFleetPlanDetailByCountryID(string countryID) {
            var fleetPlanDetailListContainer = new FleetPlanDetailListContainer();
            var con = DBManager.CreateConnection();
            var cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanDetailGetByCountry, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = countryID;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var fleetPlanEntry = new FleetPlanEntry();

                    if (reader["FleetPlanID"] != DBNull.Value)
                        fleetPlanEntry.FleetPlanID = Convert.ToInt32(reader["FleetPlanID"]);

                    if (reader["ScenarioID"] != DBNull.Value)
                        fleetPlanEntry.ScenarioID = Convert.ToInt32(reader["ScenarioID"]);

                    fleetPlanDetailListContainer.FleetPlanEntryList.Add(fleetPlanEntry);
                }

                reader.NextResult();

                while (reader.Read()) {
                    var fleetPlanDetail = new FleetPlanDetail();

                    if (reader["fleetPlanEntryID"] != DBNull.Value)
                        fleetPlanDetail.FleetPlanEntryID = Convert.ToInt32(reader["fleetPlanEntryID"]);

                    if (reader["fleetPlanDetailID"] != DBNull.Value)
                        fleetPlanDetail.FleetPlanDetailID = Convert.ToInt32(reader["fleetPlanDetailID"]);

                    if (reader["scenarioID"] != DBNull.Value)
                        fleetPlanDetail.ScenarioID = Convert.ToInt32(reader["scenarioID"]);

                    if (reader["targetDate"] != DBNull.Value)
                        fleetPlanDetail.DateOfMovement = DateTime.Parse(reader["targetDate"].ToString());

                    if (reader["car_class_id"] != DBNull.Value)
                        fleetPlanDetail.CarGroup.CarGroupID = Convert.ToInt32(reader["car_class_id"]);

                    if (reader["car_group"] != DBNull.Value)
                        fleetPlanDetail.CarGroup.CarGroupDescription = reader["car_group"].ToString();

                    fleetPlanDetail.LocationGroup = new LocationGroup();

                    if (reader["cms_Location_Group_ID"] != DBNull.Value)
                        fleetPlanDetail.LocationGroup.LocationGroupID = Convert.ToInt32(reader["cms_Location_Group_ID"]);

                    if (reader["cms_Location_Group_Name"] != DBNull.Value)
                        fleetPlanDetail.LocationGroup.LocationGroupName = reader["cms_Location_Group_Name"].ToString();

                    if (reader["addition"] != DBNull.Value)
                        fleetPlanDetail.Addition = Convert.ToInt32(reader["addition"]);

                    if (reader["deletion"] != DBNull.Value)
                        fleetPlanDetail.Deletion = Convert.ToInt32(reader["deletion"]);

                    if (reader["amount"] != DBNull.Value)
                        fleetPlanDetail.Amount = Convert.ToInt32(reader["amount"]);

                    fleetPlanDetailListContainer.FleetPlanDetailList.Add(fleetPlanDetail);
                }
            }

            return fleetPlanDetailListContainer;
        }

        public FleetPlanDetail GetFleetPlanDetailByID(int fleetPlanDetailID) {
            var fleetPlanDetail = new FleetPlanDetail();
            var con = DBManager.CreateConnection();
            var cmd = DBManager.CreateProcedure(StoredProcedures.GetFleetPlanDetailByID, con);

            cmd.Parameters.Add(Parameters.FleetPlanDetailID, SqlDbType.Int);
            cmd.Parameters[Parameters.FleetPlanDetailID].Value = fleetPlanDetailID;
            cmd.Parameters[Parameters.FleetPlanDetailID].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    if (reader["fleetPlanEntryID"] != DBNull.Value)
                        fleetPlanDetail.FleetPlanEntryID = Convert.ToInt32(reader["fleetPlanEntryID"]);

                    if (reader["fleetPlanDetailID"] != DBNull.Value)
                        fleetPlanDetail.FleetPlanDetailID = Convert.ToInt32(reader["fleetPlanDetailID"]);

                    if (reader["targetDate"] != DBNull.Value)
                        fleetPlanDetail.DateOfMovement = DateTime.Parse(reader["targetDate"].ToString());

                    if (reader["car_class_id"] != DBNull.Value)
                        fleetPlanDetail.CarGroup.CarGroupID = Convert.ToInt32(reader["car_class_id"]);

                    if (reader["car_group"] != DBNull.Value)
                        fleetPlanDetail.CarGroup.CarGroupDescription = reader["car_group"].ToString();

                    fleetPlanDetail.LocationGroup = new LocationGroup();

                    if (reader["locationGroupID"] != DBNull.Value)
                        fleetPlanDetail.LocationGroup.LocationGroupID = Convert.ToInt32(reader["locationGroupID"]);

                    if (reader["locationGroupName"] != DBNull.Value)
                        fleetPlanDetail.LocationGroup.LocationGroupName = reader["locationGroupName"].ToString();

                    if (reader["addition"] != DBNull.Value)
                        fleetPlanDetail.Addition = Convert.ToInt32(reader["addition"]);

                    if (reader["deletion"] != DBNull.Value)
                        fleetPlanDetail.Deletion = Convert.ToInt32(reader["deletion"]);

                    if (reader["amount"] != DBNull.Value)
                        fleetPlanDetail.Amount = Convert.ToInt32(reader["amount"]);
                }
            }

            return fleetPlanDetail;
        }

        public void DeleteFleetPlanDetailByFleetPlanDetailID(int fleetPlanDetailID) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanDelete, con);

            cmd.Parameters.Add(Parameters.FleetPlanDetailID, SqlDbType.Int);
            cmd.Parameters[Parameters.FleetPlanDetailID].Value = fleetPlanDetailID;
            cmd.Parameters[Parameters.FleetPlanDetailID].Direction = ParameterDirection.Input;


            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void FleetPlanBulkInsert(string user, string originalFileName, string archiveFileName, int fleetPlanID, string country, bool isAddition) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanBulkInsert, con);

            cmd.Parameters.Add(Parameters.User, SqlDbType.VarChar, 50);
            cmd.Parameters[Parameters.User].Value = user;
            cmd.Parameters[Parameters.User].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.Filename, SqlDbType.VarChar, 50);
            cmd.Parameters[Parameters.Filename].Value = originalFileName;
            cmd.Parameters[Parameters.Filename].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.ArchiveFilename, SqlDbType.VarChar, 500);
            cmd.Parameters[Parameters.ArchiveFilename].Value = archiveFileName;
            cmd.Parameters[Parameters.ArchiveFilename].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.FleetPlanID, SqlDbType.Int);
            cmd.Parameters[Parameters.FleetPlanID].Value = fleetPlanID;
            cmd.Parameters[Parameters.FleetPlanID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = country;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.IsAddition, SqlDbType.Bit);
            cmd.Parameters[Parameters.IsAddition].Value = isAddition;
            cmd.Parameters[Parameters.IsAddition].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void FleetPlanEntryUploadArchiveCreate(string user, string originalFileName, string archiveFileName, int fleetPlanID, string country, bool isAddition) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanEntryUploadArchiveCreate, con);

            cmd.Parameters.Add(Parameters.User, SqlDbType.VarChar, 100);
            cmd.Parameters[Parameters.User].Value = user;
            cmd.Parameters[Parameters.User].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.Filename, SqlDbType.VarChar, 50);
            cmd.Parameters[Parameters.Filename].Value = originalFileName;
            cmd.Parameters[Parameters.Filename].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.ArchiveFilename, SqlDbType.VarChar, 500);
            cmd.Parameters[Parameters.ArchiveFilename].Value = archiveFileName;
            cmd.Parameters[Parameters.ArchiveFilename].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.FleetPlanID, SqlDbType.Int);
            cmd.Parameters[Parameters.FleetPlanID].Value = fleetPlanID;
            cmd.Parameters[Parameters.FleetPlanID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = country;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.IsAddition, SqlDbType.Bit);
            cmd.Parameters[Parameters.IsAddition].Value = isAddition;
            cmd.Parameters[Parameters.IsAddition].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void FleetPlanUpdate(FleetPlanDetail fleetPlanDetail) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanMovementUpdate, con);

            cmd.Parameters.Add(Parameters.FleetPlanEntryID, SqlDbType.Int);
            cmd.Parameters[Parameters.FleetPlanEntryID].Value = fleetPlanDetail.FleetPlanEntryID;
            cmd.Parameters[Parameters.FleetPlanEntryID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.FleetPlanDetailID, SqlDbType.Int);
            cmd.Parameters[Parameters.FleetPlanDetailID].Value = fleetPlanDetail.FleetPlanDetailID;
            cmd.Parameters[Parameters.FleetPlanDetailID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.CarClass, SqlDbType.VarChar);
            cmd.Parameters[Parameters.CarClass].Value = fleetPlanDetail.CarGroup.CarGroupID;
            cmd.Parameters[Parameters.CarClass].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.FleetPlanLocationGroupID, SqlDbType.Int);
            cmd.Parameters[Parameters.FleetPlanLocationGroupID].Value = fleetPlanDetail.LocationGroup.LocationGroupID;
            cmd.Parameters[Parameters.FleetPlanLocationGroupID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.TargetDate, SqlDbType.DateTime);
            cmd.Parameters[Parameters.TargetDate].Value = fleetPlanDetail.DateOfMovement;
            cmd.Parameters[Parameters.TargetDate].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.Addition, SqlDbType.Int);
            cmd.Parameters[Parameters.Addition].Value = fleetPlanDetail.Addition;
            cmd.Parameters[Parameters.Addition].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.Deletion, SqlDbType.Int);
            cmd.Parameters[Parameters.Deletion].Value = fleetPlanDetail.Deletion;
            cmd.Parameters[Parameters.Deletion].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }

        }

        public List<FleetPlanEntryArchive> FleetPlanEntryUploadArchiveGetByCountry(string countryID) {
            List<FleetPlanEntryArchive> fleetPlanEntryArchiveList = new List<FleetPlanEntryArchive>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanEntryUploadArchiveGetBy, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = countryID;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;
            using (con) {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var fleetPlanEntryarchive = new FleetPlanEntryArchive();

                    if (reader["PKID"] != DBNull.Value)
                        fleetPlanEntryarchive.PKID = Convert.ToInt32(reader["PKID"]);

                    if (reader["country_description"] != DBNull.Value)
                        fleetPlanEntryarchive.Country = reader["country_description"].ToString();

                    if (reader["planDescription"] != DBNull.Value)
                        fleetPlanEntryarchive.FleetPlan = reader["planDescription"].ToString();

                    if (reader["uploadedBy"] != DBNull.Value)
                        fleetPlanEntryarchive.UploadedBy = reader["uploadedBy"].ToString();

                    if (reader["uploadedDate"] != DBNull.Value)
                        fleetPlanEntryarchive.UploadedDate = DateTime.Parse(reader["uploadedDate"].ToString());

                    if (reader["uploadedFileName"] != DBNull.Value)
                        fleetPlanEntryarchive.UploadedFileName = reader["uploadedFileName"].ToString();

                    if (reader["archiveFileName"] != DBNull.Value)
                        fleetPlanEntryarchive.UploadedArchiveFileName = reader["archiveFileName"].ToString();

                    if (reader["isAddition"] != DBNull.Value)
                        fleetPlanEntryarchive.IsAddition = Convert.ToBoolean(reader["isAddition"]);

                    fleetPlanEntryArchiveList.Add(fleetPlanEntryarchive);
                }
            }

            return fleetPlanEntryArchiveList;
        }

        public List<FrozenZoneAcceptance> FrozenZoneAcceptanceGetBy(string countryID) {
            List<FrozenZoneAcceptance> frozenZoneAcceptanceList = new List<FrozenZoneAcceptance>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FrozenZoneAcceptanceLogGetByCountry, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = countryID;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var frozenZoneAcceptance = new FrozenZoneAcceptance();

                    if (reader["PKID"] != DBNull.Value)
                        frozenZoneAcceptance.PKID = Convert.ToInt32(reader["PKID"]);

                    if (reader["Country"] != DBNull.Value)
                        frozenZoneAcceptance.Country = reader["Country"].ToString();

                    if (reader["Year"] != DBNull.Value)
                        frozenZoneAcceptance.Year = reader["Year"].ToString();

                    //if (reader["planDescription"] != DBNull.Value)
                    //    frozenZoneAcceptance.FleetPlan = reader["planDescription"].ToString();

                    if (reader["acceptedBy"] != DBNull.Value)
                        frozenZoneAcceptance.AcceptedBy = reader["acceptedBy"].ToString();

                    if (reader["acceptedDate"] != DBNull.Value)
                        frozenZoneAcceptance.AcceptedDate = DateTime.Parse(reader["acceptedDate"].ToString());

                    if (reader["acceptedWeekNumber"] != DBNull.Value)
                        frozenZoneAcceptance.AcceptedWeekNumber = Convert.ToInt32(reader["acceptedWeekNumber"]);

                    frozenZoneAcceptanceList.Add(frozenZoneAcceptance);
                }
            }

            return frozenZoneAcceptanceList;
        }

        public void UpdateFrozenForecast(string country, DateTime fromDate, DateTime toDate) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.UpdateFrozenForecast, con);

            cmd.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.Country].Value = country;
            cmd.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.FromDate, SqlDbType.DateTime);
            cmd.Parameters[Parameters.FromDate].Value = fromDate;
            cmd.Parameters[Parameters.FromDate].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.ToDate, SqlDbType.DateTime);
            cmd.Parameters[Parameters.ToDate].Value = toDate;
            cmd.Parameters[Parameters.ToDate].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateFrozenForecastAcceptanceLog(string country, string year, string acceptedBy, int weekNumber) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.UpdateFrozenZoneAcceptanceLog, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = country;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.Year, SqlDbType.VarChar, 10);
            cmd.Parameters[Parameters.Year].Value = year;
            cmd.Parameters[Parameters.Year].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.User, SqlDbType.VarChar, 50);
            cmd.Parameters[Parameters.User].Value = acceptedBy;
            cmd.Parameters[Parameters.User].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.WeekNumber, SqlDbType.Int);
            cmd.Parameters[Parameters.WeekNumber].Value = weekNumber;
            cmd.Parameters[Parameters.WeekNumber].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<NecessaryFleet> GetNecessaryFleetByCountryID(string countryID) {
            var necessaryFleetList = new List<NecessaryFleet>();
            var con = DBManager.CreateConnection();
            var cmd = DBManager.CreateProcedure(StoredProcedures.NecessaryFleetGetByCountry, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = countryID;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var necessaryFleet = new NecessaryFleet();

                    if (reader["COUNTRY"] != DBNull.Value)
                        necessaryFleet.Country.CountryID = (reader["COUNTRY"].ToString());

                    if (reader["country_description"] != DBNull.Value)
                        necessaryFleet.Country.CountryDescription = (reader["country_description"].ToString());

                    if (reader["cms_location_group_id"] != DBNull.Value)
                        necessaryFleet.LocationGroup.LocationGroupID = Convert.ToInt32(reader["cms_location_group_id"]);

                    if (reader["cms_location_group"] != DBNull.Value)
                        necessaryFleet.LocationGroup.LocationGroupName = (reader["cms_location_group"].ToString());

                    if (reader["car_group_id"] != DBNull.Value)
                        necessaryFleet.CarGroup.CarGroupID = Convert.ToInt32(reader["car_group_id"]);

                    if (reader["car_group"] != DBNull.Value)
                        necessaryFleet.CarGroup.CarGroupDescription = (reader["car_group"].ToString());

                    if (reader["UTILISATION"] != DBNull.Value)
                        necessaryFleet.Utilization = Convert.ToDecimal(reader["UTILISATION"]);

                    if (reader["NONREV_FLEET"] != DBNull.Value)
                        necessaryFleet.NonRevFleet = Convert.ToDecimal(reader["NONREV_FLEET"]);

                    necessaryFleetList.Add(necessaryFleet);
                }
            }

            return necessaryFleetList;
        }

        public void NecessaryFleetMultipleUpdate(string country, int locationGroupId, int carGroupID, double utilisation, double nonRev) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.NecessaryFleetMultipleUpdate, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = country;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.CarClassGroupId, SqlDbType.VarChar);
            cmd.Parameters[Parameters.CarClassGroupId].Value = carGroupID;
            cmd.Parameters[Parameters.CarClassGroupId].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.SourceLocationGroupID, SqlDbType.Int);
            cmd.Parameters[Parameters.SourceLocationGroupID].Value = locationGroupId;
            cmd.Parameters[Parameters.SourceLocationGroupID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.Utilisation, SqlDbType.Decimal);
            cmd.Parameters[Parameters.Utilisation].Value = utilisation;
            cmd.Parameters[Parameters.Utilisation].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.NonRev, SqlDbType.Decimal);
            cmd.Parameters[Parameters.NonRev].Value = nonRev;
            cmd.Parameters[Parameters.NonRev].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Country> CountryGetAllByRole(string user) {
            var countriesList = new List<Country>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.MarsV2ManagementCountriesGetAllByUser, con);

            cmd.Parameters.Add(Parameters.User, SqlDbType.VarChar, 50);
            cmd.Parameters[Parameters.User].Value = user;
            cmd.Parameters[Parameters.User].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var country = new Country();
                    if (reader["country"] != DBNull.Value)
                        country.CountryID = reader["country"].ToString();

                    if (reader["country_description"] != DBNull.Value)
                        country.CountryDescription = reader["country_description"].ToString();

                    countriesList.Add(country);
                }
            }

            return countriesList;
        }

        public void NecessaryFleetUpdate(NecessaryFleet necessaryFleet) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.NecessaryFleetUpdate, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = necessaryFleet.Country.CountryID;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.CarClassGroupId, SqlDbType.VarChar);
            cmd.Parameters[Parameters.CarClassGroupId].Value = necessaryFleet.CarGroup.CarGroupID;
            cmd.Parameters[Parameters.CarClassGroupId].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.SourceLocationGroupID, SqlDbType.Int);
            cmd.Parameters[Parameters.SourceLocationGroupID].Value = necessaryFleet.LocationGroup.LocationGroupID;
            cmd.Parameters[Parameters.SourceLocationGroupID].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.Utilisation, SqlDbType.Decimal);
            cmd.Parameters[Parameters.Utilisation].Value = necessaryFleet.Utilization;
            cmd.Parameters[Parameters.Utilisation].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.NonRev, SqlDbType.Decimal);
            cmd.Parameters[Parameters.NonRev].Value = necessaryFleet.NonRevFleet;
            cmd.Parameters[Parameters.NonRev].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void NecessaryFleetUtilisationUpdate(string countryID, DateTime fromDate, DateTime toDate) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.NecessaryFleetUtilisationUpdate, con);

            cmd.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.Country].Value = countryID;
            cmd.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.FromDate, SqlDbType.DateTime);
            cmd.Parameters[Parameters.FromDate].Value = fromDate;
            cmd.Parameters[Parameters.FromDate].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.ToDate, SqlDbType.DateTime);
            cmd.Parameters[Parameters.ToDate].Value = toDate;
            cmd.Parameters[Parameters.ToDate].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void NecessaryFleetNonRevUpdate(string countryID, DateTime fromDate, DateTime toDate) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.NecessaryFleetNonRevUpdate, con);

            cmd.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.Country].Value = countryID;
            cmd.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.FromDate, SqlDbType.DateTime);
            cmd.Parameters[Parameters.FromDate].Value = fromDate;
            cmd.Parameters[Parameters.FromDate].Direction = ParameterDirection.Input;

            cmd.Parameters.Add(Parameters.ToDate, SqlDbType.DateTime);
            cmd.Parameters[Parameters.ToDate].Value = toDate;
            cmd.Parameters[Parameters.ToDate].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<ForecastAdjustmentEntity> ForecastAdjustmentGet(string countryID, int carSegmentID,
            int carClassGroupID, int carClassID, int cmsPoolID, int locationGroupID, DateTime date) {
            var forecastAdjustmentList = new List<ForecastAdjustmentEntity>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand command = DBManager.CreateProcedure(StoredProcedures.GetAdjustment, con);

            command.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            command.Parameters[Parameters.Country].Value = countryID;
            command.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarclassId, SqlDbType.Int);
            command.Parameters[Parameters.CarclassId].Value = carClassID;
            command.Parameters[Parameters.CarclassId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarClassGroupId, SqlDbType.Int);
            command.Parameters[Parameters.CarClassGroupId].Value = carClassGroupID;
            command.Parameters[Parameters.CarClassGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarSegmentId, SqlDbType.Int);
            command.Parameters[Parameters.CarSegmentId].Value = carSegmentID;
            command.Parameters[Parameters.CarSegmentId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.LocationGroupId, SqlDbType.Int);
            command.Parameters[Parameters.LocationGroupId].Value = locationGroupID;
            command.Parameters[Parameters.LocationGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CmsPoolId, SqlDbType.Int);
            command.Parameters[Parameters.CmsPoolId].Value = cmsPoolID;
            command.Parameters[Parameters.CmsPoolId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.Date, SqlDbType.Date);
            command.Parameters[Parameters.Date].Value = date;
            command.Parameters[Parameters.Date].Direction = ParameterDirection.Input;

            try {
                using (con) {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) {
                        var forecastAdjustment = new ForecastAdjustmentEntity();

                        var country = new Country();
                        if (reader["country"] != DBNull.Value)
                            country.CountryID = reader["country"].ToString();

                        if (reader["country_description"] != DBNull.Value)
                            country.CountryDescription = reader["country_description"].ToString();

                        var pool = new CMSPool();
                        if (reader["cms_pool_id"] != DBNull.Value)
                            pool.PoolID = Convert.ToInt32(reader["cms_pool_id"]);

                        if (reader["cms_pool"] != DBNull.Value)
                            pool.PoolDescription = reader["cms_pool"].ToString();

                        var carSegment = new CarSegment();
                        if (reader["car_segment_id"] != DBNull.Value)
                            carSegment.CarSegmentId = Convert.ToInt32(reader["car_segment_id"]);

                        if (reader["car_segment"] != DBNull.Value)
                            carSegment.CarSegmentName = reader["car_segment"].ToString();

                        var locationGroup = new LocationGroup();
                        var carClass = new CarGroup();
                        var carClassGroup = new CarClass(); //!!!db is confusing!!!

                        //begin dynamically returned columns
                        if (cmsPoolID > 0) {
                            if (reader["cms_location_group_id"] != DBNull.Value)
                                locationGroup.LocationGroupID = Convert.ToInt32(reader["cms_location_group_id"]);

                            if (reader["cms_location_group"] != DBNull.Value)
                                locationGroup.LocationGroupName = reader["cms_location_group"].ToString();
                        }

                        if (carSegmentID > 0) {
                            if (reader["CAR_CLASS_GROUP_ID"] != DBNull.Value)
                                carClassGroup.CarClassID = Convert.ToInt32(reader["CAR_CLASS_GROUP_ID"]);

                            if (reader["CAR_CLASS_GROUP"] != DBNull.Value)
                                carClassGroup.CarclassDescription = reader["CAR_CLASS_GROUP"].ToString();
                        }

                        if (carClassGroupID > 0) {
                            if (reader["CAR_CLASS_ID"] != DBNull.Value)
                                carClass.CarGroupID = Convert.ToInt32(reader["CAR_CLASS_ID"]);

                            if (reader["CAR_CLASS"] != DBNull.Value)
                                carClass.CarGroupDescription = reader["CAR_CLASS"].ToString();
                        }
                        //end dynamically returned columns

                        if (reader["ADJUSTMENT_TD"] != DBNull.Value)
                            forecastAdjustment.Adjustment_TD = Convert.ToDecimal(reader["ADJUSTMENT_TD"]);

                        if (reader["ADJUSTMENT_BU1"] != DBNull.Value)
                            forecastAdjustment.Adjustment_BU1 = Convert.ToDecimal(reader["ADJUSTMENT_BU1"]);

                        if (reader["ADJUSTMENT_BU2"] != DBNull.Value)
                            forecastAdjustment.Adjustment_BU2 = Convert.ToDecimal(reader["ADJUSTMENT_BU2"]);

                        if (reader["ADJUSTMENT_RC"] != DBNull.Value)
                            forecastAdjustment.Adjustment_RC = Convert.ToDecimal(reader["ADJUSTMENT_RC"]);

                        if (reader["ONRENT"] != DBNull.Value)
                            forecastAdjustment.OnRent = Convert.ToDecimal(reader["ONRENT"]);

                        if (reader["CONSTRAINED"] != DBNull.Value)
                            forecastAdjustment.Constrained = Convert.ToDecimal(reader["CONSTRAINED"]);

                        if (reader["UNCONSTRAINED"] != DBNull.Value)
                            forecastAdjustment.UnConstrained = Convert.ToDecimal(reader["UNCONSTRAINED"]);

                        if (reader["REP_DATE"] != DBNull.Value)
                            forecastAdjustment.Date = Convert.ToDateTime(reader["REP_DATE"]);

                        if (reader["Count"] != DBNull.Value)
                            forecastAdjustment.Count = Convert.ToInt32(reader["Count"]);

                        forecastAdjustment.Country = country;
                        forecastAdjustment.CMSPool = pool;
                        forecastAdjustment.LocationGroup = locationGroup;
                        forecastAdjustment.CarSegment = carSegment;
                        forecastAdjustment.CarClass = carClassGroup;
                        forecastAdjustment.CarGroup = carClass;

                        forecastAdjustmentList.Add(forecastAdjustment);

                    }
                }
            }
            catch {

            }

            return forecastAdjustmentList;
        }

        public void AdjustmentUpdate(int carSegmentID,
        int carClassGroupID,
        int carClassID,
        int cmsPoolID,
        int locationGroupID,
        int adjustmentToUpdate,
        int adjustmentType,
        bool addition,
        decimal adjustmentValue,
        DateTime date) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand command = DBManager.CreateProcedure(StoredProcedures.AdjustmentUpdate, con);

            command.Parameters.Add(Parameters.Date, SqlDbType.Date);
            command.Parameters[Parameters.Date].Value = date;
            command.Parameters[Parameters.Date].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.AdjustmentValue, SqlDbType.Decimal);
            command.Parameters[Parameters.AdjustmentValue].Value = adjustmentValue;
            command.Parameters[Parameters.AdjustmentValue].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.LocationGroupId, SqlDbType.Int);
            command.Parameters[Parameters.LocationGroupId].Value = locationGroupID;
            command.Parameters[Parameters.LocationGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CmsPoolId, SqlDbType.Int);
            command.Parameters[Parameters.CmsPoolId].Value = cmsPoolID;
            command.Parameters[Parameters.CmsPoolId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarclassId, SqlDbType.Int);
            command.Parameters[Parameters.CarclassId].Value = carClassID;
            command.Parameters[Parameters.CarclassId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarClassGroupId, SqlDbType.Int);
            command.Parameters[Parameters.CarClassGroupId].Value = carClassGroupID;
            command.Parameters[Parameters.CarClassGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarSegmentId, SqlDbType.Int);
            command.Parameters[Parameters.CarSegmentId].Value = carSegmentID;
            command.Parameters[Parameters.CarSegmentId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.AdjustmentToUpdate, SqlDbType.Int);
            command.Parameters[Parameters.AdjustmentToUpdate].Value = adjustmentToUpdate;
            command.Parameters[Parameters.AdjustmentToUpdate].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.AdjustmentType, SqlDbType.Int);
            command.Parameters[Parameters.AdjustmentType].Value = adjustmentType;
            command.Parameters[Parameters.AdjustmentType].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.IsAddition, SqlDbType.Bit);
            command.Parameters[Parameters.IsAddition].Value = addition;
            command.Parameters[Parameters.IsAddition].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AdjustmentAdapt(int carSegmentID,
        int carClassGroupID,
        int carClassID,
        int cmsPoolID,
        int locationGroupID,
        AdjustmentForecast from,
        Adjustment to,
        DateTime date) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand command = DBManager.CreateProcedure(StoredProcedures.AdjustmentAdapt, con);

            command.Parameters.Add(Parameters.Date, SqlDbType.Date);
            command.Parameters[Parameters.Date].Value = date;
            command.Parameters[Parameters.Date].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.LocationGroupId, SqlDbType.Int);
            command.Parameters[Parameters.LocationGroupId].Value = locationGroupID;
            command.Parameters[Parameters.LocationGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CmsPoolId, SqlDbType.Int);
            command.Parameters[Parameters.CmsPoolId].Value = cmsPoolID;
            command.Parameters[Parameters.CmsPoolId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarclassId, SqlDbType.Int);
            command.Parameters[Parameters.CarclassId].Value = carClassID;
            command.Parameters[Parameters.CarclassId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarClassGroupId, SqlDbType.Int);
            command.Parameters[Parameters.CarClassGroupId].Value = carClassGroupID;
            command.Parameters[Parameters.CarClassGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarSegmentId, SqlDbType.Int);
            command.Parameters[Parameters.CarSegmentId].Value = carSegmentID;
            command.Parameters[Parameters.CarSegmentId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.AdjustmentToUpdate, SqlDbType.Int);
            command.Parameters[Parameters.AdjustmentToUpdate].Value = (int)to;
            command.Parameters[Parameters.AdjustmentToUpdate].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.ForecastToUpdateFrom, SqlDbType.Int);
            command.Parameters[Parameters.ForecastToUpdateFrom].Value = (int)from;
            command.Parameters[Parameters.ForecastToUpdateFrom].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AdjustmentReconcile(int carSegmentID,
        int carClassGroupID,
        int carClassID,
        int cmsPoolID,
        int locationGroupID,
        Adjustment from,
        DateTime date) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand command = DBManager.CreateProcedure(StoredProcedures.AdjustmentReconcile, con);

            command.Parameters.Add(Parameters.Date, SqlDbType.Date);
            command.Parameters[Parameters.Date].Value = date;
            command.Parameters[Parameters.Date].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.LocationGroupId, SqlDbType.Int);
            command.Parameters[Parameters.LocationGroupId].Value = locationGroupID;
            command.Parameters[Parameters.LocationGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CmsPoolId, SqlDbType.Int);
            command.Parameters[Parameters.CmsPoolId].Value = cmsPoolID;
            command.Parameters[Parameters.CmsPoolId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarclassId, SqlDbType.Int);
            command.Parameters[Parameters.CarclassId].Value = carClassID;
            command.Parameters[Parameters.CarclassId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarClassGroupId, SqlDbType.Int);
            command.Parameters[Parameters.CarClassGroupId].Value = carClassGroupID;
            command.Parameters[Parameters.CarClassGroupId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.CarSegmentId, SqlDbType.Int);
            command.Parameters[Parameters.CarSegmentId].Value = carSegmentID;
            command.Parameters[Parameters.CarSegmentId].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.AdjustmentToUpdateFrom, SqlDbType.Int);
            command.Parameters[Parameters.AdjustmentToUpdateFrom].Value = (int)from;
            command.Parameters[Parameters.AdjustmentToUpdateFrom].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<UserRole> UsersInRolesGet(string userID) {
            var userRoleList = new List<UserRole>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand command = DBManager.CreateProcedure(StoredProcedures.UsersInRolesSelectAll, con);

            command.Parameters.Add(Parameters.Racfid, SqlDbType.VarChar, 10);
            command.Parameters[Parameters.Racfid].Value = userID;
            command.Parameters[Parameters.Racfid].Direction = ParameterDirection.Input;

            using (con) {
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    var userRole = new UserRole();

                    if (reader["racfId"] != DBNull.Value)
                        userRole.UserID = reader["racfId"].ToString();

                    if (reader["name"] != DBNull.Value)
                        userRole.UserName = reader["name"].ToString();

                    if (reader["country"] != DBNull.Value)
                        userRole.RoleCountry = reader["country"].ToString();

                    if (reader["roleName"] != DBNull.Value)
                        userRole.RoleName = reader["roleName"].ToString();

                    if (reader["description"] != DBNull.Value)
                        userRole.RoleDescription = reader["description"].ToString();

                    if (reader["pageAccess"] != DBNull.Value)
                        userRole.RolePageAccess = reader["pageAccess"].ToString();

                    if (reader["id"] != DBNull.Value)
                        userRole.RoleID = Convert.ToInt32(reader["id"]);

                    userRoleList.Add(userRole);
                }
            }

            return userRoleList;
        }

        public void ForecastOperationalFleetUpdate(DateTime date, int fleetplanID, string country) {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand command = DBManager.CreateProcedure(StoredProcedures.ForecastOperationalFleetUpdate, con);
            
           

            command.Parameters.Add(Parameters.TheDay, SqlDbType.Date);
            command.Parameters[Parameters.TheDay].Value = date;
            command.Parameters[Parameters.TheDay].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.FleetPlanID, SqlDbType.Int);
            command.Parameters[Parameters.FleetPlanID].Value = fleetplanID;
            command.Parameters[Parameters.FleetPlanID].Direction = ParameterDirection.Input;

            command.Parameters.Add(Parameters.Country, SqlDbType.VarChar);
            command.Parameters[Parameters.Country].Value = country;
            command.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            ThreadPool.QueueUserWorkItem(delegate {
                using (con)
                {
                    con.Open();



                    command.BeginExecuteNonQuery();
                }
            });

            
        }

    }
}

namespace App.DAL.Data
{
    public static class StoredProcedures
    {
        public const string CountriesSelect = @"spCountriesSelect";
        public const string CMSPoolsSelect = @"spCMSPoolsSelect";
        public const string Portal_CMSPoolSelectWithCountry = @"spPortal_CMSPoolSelectWithCountry";
        public const string CMSLocationGroupsSelect = @"spCMSLocationGroupsSelect";
        public const string Portal_CMSLocationGroupSelectWithCMSPool = @"spPortal_CMSLocationGroupSelectWithCMSPool";
        public const string LocationsSelect = @"spLocationsSelect";
        public const string OPSRegionsSelect = @"spOPSRegionsSelect";
        public const string Portal_OPSRegionSelectWithCountry = @"spPortal_OPSRegionSelectWithCountry";
        public const string CarGroupsSelect = @"spCarGroupsSelect";
        public const string OPSAreasSelect = @"spOPSAreasSelect";
        public const string Portal_OPSAreaWithOPSRegion = @"spPortal_OPSAreaWithOPSRegion";
        public const string FleetLookupSelect = @"spFleetLookupSelect";
        public const string CarSegmentsSelect = @"spCarSegmentsSelect";
        public const string Portal_CarSegmentSelectWithCountry = @"spPortal_CarSegmentSelectWithCountry";
        public const string CarClassesSelect = @"spCarClassesSelect";
        public const string Portal_CarClassSelectWithCarSegment = @"spPortal_CarClassSelectWithCarSegment";
        public const string OperstatsSelect = @"spOperstatsSelect";
        public const string OperstatsSelect_Settings = @"spOperstatsSelect_Settings";
        public const string ModelcodeSelect = @"spModelcodeSelect";
        public const string OwnareaSelect = @"spOwnareaSelect";
        public const string MovTypeSelect = @"spMovTypeSelect";
        public const string RemarkListSelect = @"spRemarkListSelect";
        public const string DayGroupCodeSelect = @"spDayGroupCodeSelect";

        public const string Mars_UsageStatisticsInsert = @"spMars_UsageStatisticsInsert";
        public const string MARS_UsageStatisticsBySelection = @"spMARS_UsageStatisticsBySelection";
        public const string MARS_UsageStatisticsByDate = @"spMARS_UsageStatisticsByDate";

        public const string ExportLevelFleetGetAll = @"ExportLevelFleetGetAll";
        public const string ExportLevelSiteGetAll = @"ExportLevelSiteGetAll";
        
        public const string ReportingTimeZoneGetAll = @"proc_ReportingTimeZoneGetAll";
        public const string CMSForecastTypeGetAll = @"proc_CMSForecastTypeGetAll";
        public const string CMSFleetPlanGetAll = @"proc_CMSFleetPlanGetAll";

        public const string CMSCountryGetAll = @"proc_MarsCountriesGetAll";
        public const string LocationGroupGetByCountryID = @"proc_LocationGroupGetByCountryID";
        public const string CarClassGetByCountryID = @"spCarClassesSelect";
        public const string CarGroupGetByCountryID = @"spCarGroupsSelect";

        public const string GetFutureTrendExportData = @"proc_GetFutureTrendExportData";
        public const string GetSupplyAnalysisExportData = @"proc_GetSupplyAnalysisExportData";
        public const string GetFleetComparisonExportData = @"proc_GetFleetComparisonExportData";
        public const string GetSiteComparisonExportData = @"proc_GetSiteComparisonExportData";
        public const string GetKPIExportData = @"proc_GetKPIExportData";
        public const string GetNecessaryFleetExportData = @"proc_GetNecessaryFleetExportData";
        public const string GetForecastExportData = @"proc_GetForecastExportData";
        public const string GetBenchmarkExportData = @"proc_GetBenchmarkExportData";
        public const string GetFutureTrendReportData = @"proc_GetFutureTrendReportData";
        public const string GetSupplyAnalysisReportData = @"proc_GetSupplyAnalysisReportData";

        public const string FleetPlanDetailGetByCountry = @"proc_FleetPlanDetailGetByCountry";
        public const string GetFleetPlanDetailBy = @"proc_FleetPlanDetailGetBy";
        public const string GetFleetPlanDetailByID = @"proc_FleetPlanDetailGetByID";
        public const string FleetPlanDelete = @"proc_FleetplanDelete";
        public const string FleetPlanMovementUpdate = @"proc_FleetPlanMovementUpdate";
        public const string FleetPlanBulkInsert = @"proc_FleetPlanEntryBulkInsert";
        public const string FleetPlanEntryUploadArchiveGetBy = @"proc_FleetPlanEntryUploadArchiveGetBy";
        public const string FleetPlanDetailExport = @"proc_FleetPlanDetailExport";
        public const string FleetPlanEntryUploadArchiveCreate = @"proc_FleetPlanEntryUploadArchiveCreate";
        

        public const string ForecastOperationalFleetUpdate = @"spSSISCMSForecastOperationalFleetUpdate";

        public const string UpdateFrozenForecast = @"spSSISCMSForecastHistoryUpdateFrozenForecast";
        public const string UpdateFrozenZoneAcceptanceLog = @"proc_MARS_CMS_FrozenZoneAcceptanceLog_Update";
        public const string FrozenZoneAcceptanceLogGetByCountry = @"proc_MARS_CMS_FrozenZoneAcceptanceLog_GetByCountry";

        public const string NecessaryFleetGetByCountry = @"proc_MARS_CMS_Necessayfleet_GetByCountry";
        public const string NecessaryFleetMultipleUpdate = @"proc_MARS_CMS_NecessaryFleet_MultipleUpdate";
        public const string NecessaryFleetUpdate = @"proc_MARS_CMS_NecessaryFleet_Update";
        public const string NecessaryFleetUtilisationUpdate = @"proc_NecessaryFleetUtilisationUpdate";
        public const string NecessaryFleetNonRevUpdate = @"proc_NecessaryFleetNonRevUpdate";
        
        public const string MarsV2ManagementCountriesGetAllByUser = @"proc_MarsV2ManagementCountriesGetAllByUser";
        public const string UsersInRolesSelectAll = @"proc_UsersInRolesSelectAll";

        public const string ForecastAdjustmentByCountry = @"proc_ForecastAdjustmentByCountry";
        public const string GetAdjustment = @"proc_GetAdjustment";
        public const string AdjustmentUpdate = @"proc_AdjustmentUpdate";
        public const string AdjustmentAdapt = @"proc_AdjustmentAdapt";
        public const string AdjustmentReconcile = @"proc_AdjustmentReconcile";       

        public const string GetLastImportTimeByImportType = @"spGetLastImportTimeByImportType";
        public const string GetNextImportTimeByImportType = @"spGetNextImportTimeByImportType";

        public const string Availability_ReportCarSearch = @"spReportCarSearch";
        public const string Availability_VehicleRemarksSelect = @"spVehicleRemarksSelect";
        public const string Availability_VehicleRemarksInsertUpdate = @"spVehicleRemarksInsertUpdate";

        public const string VehiclesLease_Insert = @"[General].[Insert_Vehicles_Lease]";
        public const string VehiclesLease_Update = @"[General].[Update_Vehicles_Lease]";
        public const string VehiclesLease_Delete = @"[General].[Delete_Vehicles_Lease]";
        public const string VehiclesLease_Select_GridView = @"[General].[Select_Vehicles_Lease_Gridview]";
        public const string VehiclesLease_Select_CheckExist = @"[General].[Select_Vehicles_Lease_CheckExist]";
        public const string VehiclesLease_Select_BySerial = @"[General].[Select_Vehicles_Lease_BySerial]";
        public const string VehiclesLease_Select_ModelDescription = @"[General].[Select_Vehicles_Lease_ModelDescription]";

        public const string FleetSerial_Select_ByCountry = @"[General].[Select_Fleet_Serial_ByCountry]";

        public const string Portal_CMSLocationGroupSelect = @"spPortal_CMSLocationGroupSelect";
        public const string Portal_CMSLocationGroupInsert = @"spPortal_CMSLocationGroupInsert";
        public const string Portal_CMSLocationGroupUpdate = @"spPortal_CMSLocationGroupUpdate";
        public const string Portal_CMSLocationGroupDelete = @"spPortal_CMSLocationGroupDelete";
        public const string Portal_CMSLocationGroupSelectByCode = @"spPortal_CMSLocationGroupSelectByCode";
        public const string Portal_CMSLocationGroupCheckExists = @"spPortal_CMSLocationGroupCheckExists";

        public const string Portal_CMSPoolSelect = @"spPortal_CMSPoolSelect";
        public const string Portal_CMSPoolInsert = @"spPortal_CMSPoolInsert";
        public const string Portal_CMSPoolUpdate = @"spPortal_CMSPoolUpdate";
        public const string Portal_CMSPoolDelete = @"spPortal_CMSPoolDelete";
        public const string Portal_CMSPoolSelectById = @"spPortal_CMSPoolSelectById";

        public const string Portal_AreaCodeSelect = @"spPortal_AreaCodeSelect";
        public const string Portal_AreaCodeCheckExists = @"spPortal_AreaCodeCheckExists";
        public const string Portal_AreaCodeInsert = @"spPortal_AreaCodeInsert";
        public const string Portal_AreaCodeUpdate = @"spPortal_AreaCodeUpdate";
        public const string Portal_AreaCodeDelete = @"spPortal_AreaCodeDelete";
        public const string Portal_AreaCodeSelectByOwnArea = @"spPortal_AreaCodeSelectByOwnArea";

        public const string Portal_CarClassSelect = @"spPortal_CarClassSelect";
        public const string Portal_CarClassInsert = @"spPortal_CarClassInsert";
        public const string Portal_CarClassUpdate = @"spPortal_CarClassUpdate";
        public const string Portal_CarClassDelete = @"spPortal_CarClassDelete";
        public const string Portal_CarClassSelectById = @"spPortal_CarClassSelectById";

        public const string Portal_CarGroupSelect = @"spPortal_CarGroupSelect";
        public const string Portal_CarGroupInsert = @"spPortal_CarGroupInsert";
        public const string Portal_CarGroupUpdate = @"spPortal_CarGroupUpdate";
        public const string Portal_CarGroupSelectById = @"spPortal_CarGroupSelectById";
        public const string Portal_CarGroupDelete = @"spPortal_CarGroupDelete";

        public const string Portal_CarSegmentSelect = @"spPortal_CarSegmentSelect";
        public const string Portal_CarSegmentInsert = @"spPortal_CarSegmentInsert";
        public const string Portal_CarSegmentUpdate = @"spPortal_CarSegmentUpdate";
        public const string Portal_CarSegmentDelete = @"spPortal_CarSegmentDelete";
        public const string Portal_CarSegmentSelectById = @"spPortal_CarSegmentSelectById";

        public const string Portal_CountrySelect = @"spPortal_CountrySelect";
        public const string Portal_CountryCheckExists = @"spPortal_CountryCheckExists";
        public const string Portal_CountryInsert = @"spPortal_CountryInsert";
        public const string Portal_CountryDelete = @"spPortal_CountryDelete";
        public const string Portal_CountryUpdate = @"spPortal_CountryUpdate";
        public const string Portal_CountrySelectByCountry = @"spPortal_CountrySelectByCountry";
        public const string Portal_CountriesSelectAll = @"spPortal_CountriesSelectAll";

        public const string Portal_LocationSelect = @"spPortal_LocationSelect";
        public const string Portal_LocationInsert = @"spPortal_LocationInsert";
        public const string Portal_LocationUpdate = @"spPortal_LocationUpdate";
        public const string Portal_LocationDelete = @"spPortal_LocationDelete";
        public const string Portal_LocationSelectByLocation = @"spPortal_LocationSelectByLocation";
        public const string Portal_LocationCheckExists = @"spPortal_LocationCheckExists";

        public const string Portal_ModelCodeSelect = @"spPortal_ModelCodeSelect";
        public const string Portal_ModelCodeInsert = @"spPortal_ModelCodeInsert";
        public const string Portal_ModelCodeUpdate = @"spPortal_ModelCodeUpdate";
        public const string Portal_ModelCodeDelete = @"spPortal_ModelCodeDelete";
        public const string Portal_ModelCodeSelectById = @"spPortal_ModelCodeSelectById";

        public const string Portal_OPSAreaSelect = @"spPortal_OPSAreaSelect";
        public const string Portal_OPSAreaInsert = @"spPortal_OPSAreaInsert";
        public const string Portal_OPSAreaUpdate = @"spPortal_OPSAreaUpdate";
        public const string Portal_OPSAreaDelete = @"spPortal_OPSAreaDelete";
        public const string Portal_OPSAreaSelectById = @"spPortal_OPSAreaSelectById";

        public const string Portal_OPSRegionSelect = @"spPortal_OPSRegionSelect";
        public const string Portal_OPSRegionInsert = @"spPortal_OPSRegionInsert";
        public const string Portal_OPSRegionUpdate = @"spPortal_OPSRegionUpdate";
        public const string Portal_OPSRegionDelete = @"spPortal_OPSRegionDelete";
        public const string Portal_OPSRegionSelectById = @"spPortal_OPSRegionSelectById";

        public const string Portal_RolesSelectAll = @"spPortal_RolesSelectAll";
        public const string Portal_RolesSelectDescriptions = @"spPortal_RolesSelectDescriptions";
        public const string Portal_RolesSelectForUser = @"spPortal_RolesSelectForUser";
        public const string Portal_UserDeleteRoles = @"spPortal_UserDeleteRoles";
        public const string Portal_UserCheckExists = @"spPortal_UserCheckExists";
        public const string Portal_UserSelectAll = @"spPortal_UserSelectAll";
        public const string Portal_UserSelectFilter = @"Spportal_userselectfilter";
        public const string Portal_UserInsert = @"spPortal_UserInsert";
        public const string Portal_UserUpdate = @"spPortal_UserUpdate";
        public const string Portal_UserUpdateRoles = @"spPortal_UserUpdateRoles";
        public const string Portal_UserDelete = @"spPortal_UserDelete";


        public const string NonRev_Select_ModelCodes = @"[General].[Select_NonRev_ModelCodes]";
        public const string NonRev_ReportCarSearch_Grid = @"[General].[Select_NonRev_Grid]";
        public const string NonRev_ReportCarSearch_Grid_Excel = @"[General].[Select_NonRev_Grid_Excel]";
        public const string Nonrev_ReportCarSearch_Form = @"[General].[Select_NonRev_Form]";
        public const string NonRev_ReportCarSearch_Form_Serial = @"[General].[Select_NonRev_Form_Serial]";
        public const string NonRev_ReportCarSearch_Grid_History = @"[General].[Select_NonRev_Grid_History]";
        public const string NonRev_ReportStart_Grid = @"[General].[Select_NonRev_Grid_Start]";
        public const string NonRev_ReportStart_GridRemark = @"[General].[Select_NonRev_Grid_Start_Remarks]";
        public const string NonRev_ReportStart_Chart = @"[General].[Select_NonRev_Chart_Start]";
        public const string NonRev_Insert_Remark = @"[General].[Insert_NonRev_Remarks]";
        public const string NonRev_Approval_Grid = @"[General].[Select_NonRev_Grid_Approval]";
        public const string NonRev_Approval_Grid_UserCars = @"[General].[Select_NonRev_Grid_Approval_UserCars]";
        public const string MonRev_Approval_Grid_UserCars_Excel = @"[General].[Select_NonRev_Grid_Approval_UserCars_Excel]";
        public const string NonRev_Approval_Grid_User = @"[General].[Select_NonRev_Grid_Approval_User]";
        public const string NonRev_Approval_Excel = @"[General].[Select_NonRev_Grid_Approval_Excel]";
        public const string NonRev_Approval_Update = @"[General].[Update_NonRev_Approval]";

        public const string NonRev_Comparison = @"[General].[Select_NonRev_Chart_Comparison]";
        public const string NonRev_Comparison_Grid = @"[General].[Select_NonRev_Grid_Comparison]";

        public const string NonRev_HTrend_Grid = @"[General].[Select_NonRev_Grid_HistoricalTrend]";
        public const string NonRev_HTrend_Chart = @"[General].[Select_NonRev_Chart_HistoricalTrend]";

        public const string SearchTermSerial = @"[General].[Select_NonRev_SearchTerm_Serial]";
        public const string SearchTermPlate = @"[General].[Select_NonRev_SearchTerm_Serial]";
        public const string SearchTermDriver = @"[General].[Select_NonRev_SearchTerm_Serial]";

        public const string NonRev_Remarks_Edit = @"[Settings].[NonRev_Remark_Edit]";

        public const string NonRev_ImportFile_Truncate = @"[Import].[Truncate_NonRevFile]";
        public const string NonRev_ImportFile_Insert = @"[Import].[Insert_NonRevFile]";
        public const string NonRev_Update_RemarkAll = @"[Import].[Update_NonRevRemarks]";
        public const string NonRev_ImportFile_UpdateTables = @"[General].[NonRev_FileUpload]";

        public const string VehcileAutoCompleteModelDescription = "VehcileAutoCompleteModelDescription";
        public const string VehcileAutoCompleteDriverName = "VehcileAutoCompleteDriverName";
        public const string VehcileAutoCompleteVin = "VehcileAutoCompleteVin";
        public const string VehcileAutoCompleteLicencePlate = "VehcileAutoCompleteLicencePlate";
        public const string VehcileAutoCompleteUnitNumber = "VehcileAutoCompleteUnitNumber";
        public const string VehcileAutoCompleteColour = "VehcileAutoCompleteColour";

        public const string ReservationAutoCompleteExternalId = "ReservationAutoCompleteExternalId";
        public const string ReservationAutoCompleteCustomerName = "ReservationAutoCompleteCustomerName";
        public const string ReservationAutoCompleteFlightNumber = "ReservationAutoCompleteFlightNumber";


        public const string AutoCompleteLocationWwdCode = "AutoCompleteLocationWwdCode";

        public const string VehcileAutoCompleteAreaCode = "VehcileAutoCompleteAreaCode";


        public const string CMSPoolSelect = "[spAutoComCmsPools]";
        public const string CMSPoolAutoComplete = "[spCMSPoolAutoComplete]";
        public const string CMSLocGroupAutoComplete = "[spCmsLocGroupAutoComplete]";
        public const string OpsAreaAutoComplete = "[spOpsAreaAutoComplete]";
        public const string OpsRegionAutoComplete = "[spOpsRegionAutoComplete]";
        public const string CarSegmentsAutoComplete = "[spCarSegmentsAutoComplete]";
        public const string CarClassesAutoComplete = "[spCarClassesAutoComplete]";
        public const string CarGroupAutoComplete = "[spCarGroupsAutoComplete]";
        public const string LocationinCMSLocGrpAutoComplete = "[spLocationAutoComplete]";
        public const string LocationinOPSAreaAutoComplete = "[spLocationbyOpsAreaAutoComplete]";
        public const string LocationCodeinCMSLocGrpAutoComplete = "[spLocationCodebyCmsLocGrp]";
        public const string LocationCodeinOPSAreaAutoComplete = "[spLocationCodebyOpsArea]";
        public const string UsersbyRacfIdAutoComplete = "[spUserswithRacfid]";
        public const string UsersbyNameAutoComplete = "[spUserSearchbyName]";
       
    }

    public static class Parameters
    {
        public const string Country = @"@country";
        public const string CountryID = @"@countryID";
        public const string FleetPlanID = @"@fleetPlanID";
        public const string FleetPlanEntryID = @"@fleetPlanEntryID";
        public const string FleetPlanDetailID = @"@FleetPlanDetailID";
        public const string User = @"@User";
        public const string Filename = @"@originalFileName";
        public const string ArchiveFilename = @"@archiveFileName";
        public const string IsAddition = @"@isAddition";
        public const string Year = @"@year";

        public const string TargetDate = @"@TargetDate";
        public const string CarClass = @"@car_class";
        public const string FleetPlanLocationGroupID = @"@cms_Location_Group_ID";
        public const string SourceLocationGroupID = @"@srcLocationGroupID";
        public const string Amount = @"@amount";
        public const string Addition = @"@addition";
        public const string Deletion = @"@deletion";

        public const string CarSegmentId = @"@carSegmentID";
        public const string CarClassGroupId = @"@carClassGroupID";
        public const string CarclassId = @"@carclassID";
        public const string CmsPoolId = @"@cmsPoolID";
        public const string CmsLocationGroupId = @"@cmsLocationroupID";
        public const string LocationGroupId = @"@locationGroupID";
        public const string ForecastType = @"@forecastType";
        public const string Timezone = @"@timezone";
        public const string FleetPlan = @"@fleetPlan";
        
        public const string StartDate = @"@startDate";
        public const string EndDate = @"@endDate";
        public const string FromDate = @"@dateFrom";
        public const string ToDate = @"@dateTo";
        public const string Date = @"@date";

        public const string WeekNumber = @"@weekNumber";

        public const string CarSegmentId2 = @"@car_segment_id";
        public const string CarClass2 = @"@car_class_id";

        public const string Utilisation = @"@utilisation";
        public const string NonRev = @"@nonRev";

        public const string RoleID = @"@roleID";
        public const string Racfid = @"@racfId";

        public const string AdjustmentValue = @"@adjustmentValue";
        public const string AdjustmentToUpdate = @"@adjustmentToUpdate";
        public const string ForecastToUpdateFrom = @"@forecastToUpdateFrom";
        public const string AdjustmentType = @"@adjustmentType";
        public const string AdjustmentToUpdateFrom = @"@adjustmentToUpdateFrom";

        public const string TheDay = @"@theDay";
    }
}
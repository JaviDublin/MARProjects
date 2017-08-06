using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.Pooling.AdditionDeletion;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.NonRevReWrite.ParameterHolders;

namespace Mars.App.Classes.DAL.Pooling.AdditionsDeletions
{
    public class AdditionDeletionDataAccess : IDisposable
    {
        private readonly MarsDBDataContext _dataContext;

        public AdditionDeletionDataAccess(MarsDBDataContext dc = null)
        {
            _dataContext = dc ?? new MarsDBDataContext(MarsConnection.ConnectionString);
            
        }

        public Dictionary<string,int> GetValidLocationsForCountry(string country)
        {
            var returned = _dataContext.LOCATIONs.Where(d=> d.country == country).ToDictionary(d=> d.location1, d=> d.dim_Location_id);
            return returned;
        }

        public LOCATION GetLocationId(string locationString)
        {
            var returned = _dataContext.LOCATIONs.FirstOrDefault(d => d.location1 == locationString);
            return returned;
        }

        public Dictionary<string, int> GetValidCarGroups(string country)
        {
            var carGroups = from cg in _dataContext.CAR_GROUPs
                            join cc in _dataContext.CAR_CLASSes on cg.car_class_id equals cc.car_class_id
                            join cs in _dataContext.CAR_SEGMENTs on cc.car_segment_id equals cs.car_segment_id
                            where cs.country == country
                            select cg;

            var returned = carGroups.ToDictionary(d=> d.car_group1, d=> d.car_group_id);
            return returned;
        }

        public int GetCarGroupId(string country, string carGroup)
        {
            var carGroups = from cg in _dataContext.CAR_GROUPs
                join cc in _dataContext.CAR_CLASSes on cg.car_class_id equals cc.car_class_id
                join cs in _dataContext.CAR_SEGMENTs on cc.car_segment_id equals cs.car_segment_id
                where cs.country == country && cg.car_group1 == carGroup
                select cg.car_group_id;

            var returned = carGroups.FirstOrDefault();
            return returned;
        }

        public void AttachAdditions(List<ResAddition> ra)
        {
            _dataContext.ResAdditions.AttachAll(ra);
            _dataContext.Refresh(RefreshMode.KeepCurrentValues, ra);
            
            
        }
        public void AttachDeletions(List<ResDeletion> rd)
        {
            _dataContext.ResDeletions.AttachAll(rd);
            _dataContext.Refresh(RefreshMode.KeepCurrentValues, rd);
        }

        public void InsertAddition(ResAddition ra)
        {
            _dataContext.ResAdditions.InsertOnSubmit(ra);
        }

        public void InsertDeletion(ResDeletion rd)
        {
            _dataContext.ResDeletions.InsertOnSubmit(rd);
        }

        public void InsertAdditions(List<ResAddition> ra)
        {
            _dataContext.ResAdditions.InsertAllOnSubmit(ra);
        }

        public void InsertDeletions(List<ResDeletion> rd)
        {
            _dataContext.ResDeletions.InsertAllOnSubmit(rd);
        }

        private IQueryable<ResAddition> GetAdditons(ReportsParameters selectedParams)
        {
            var returned = from ls in _dataContext.ResAdditions
                           select ls;

            if (selectedParams.SelectedDate != null)
            {
                returned = returned.Where(d => d.RepDate.Date == selectedParams.SelectedDate);
            }

            if (selectedParams.Country != null)
            {
                returned = returned.Where(d => d.LOCATION.country == selectedParams.Country); ;
            }

            if (selectedParams.CmsPoolId != null)
            {
                returned = returned.Where(d => d.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == selectedParams.CmsPoolId);
            }

            if (selectedParams.CmsLocationGroupId != null)
            {
                returned = returned.Where(d => d.LOCATION.cms_location_group_id == selectedParams.CmsLocationGroupId);
            }

            if (selectedParams.OpsAreadId != null)
            {
                returned = returned.Where(d => d.LOCATION.ops_area_id == selectedParams.OpsAreadId);
            }

            if (selectedParams.OpsRegionId != null)
            {
                returned = returned.Where(d => d.LOCATION.OPS_AREA.ops_region_id == selectedParams.OpsRegionId);
            }

            if (selectedParams.Branch != null)
            {
                returned = returned.Where(d => d.LocId == int.Parse(selectedParams.Branch));
            }

            if (selectedParams.SegmentId != null)
            {
                returned = returned.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == selectedParams.SegmentId);
            }

            if (selectedParams.ClassId != null)
            {
                returned = returned.Where(d => d.CAR_GROUP.car_class_id == selectedParams.ClassId);
            }

            if (selectedParams.GroupId != null)
            {
                returned = returned.Where(d => d.CarGrpId == selectedParams.GroupId);
            }

            return returned;
        }

        private IQueryable<ResDeletion> GetDeletions(ReportsParameters selectedParams)
        {
            var returned = from ls in _dataContext.ResDeletions
                           select ls;

            if (selectedParams.SelectedDate != null)
            {
                returned = returned.Where(d => d.RepDate.Date == selectedParams.SelectedDate);
            }

            if (selectedParams.Country != null)
            {
                returned = returned.Where(d => d.LOCATION.country == selectedParams.Country); ;
            }

            if (selectedParams.CmsPoolId != null)
            {
                returned = returned.Where(d => d.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == selectedParams.CmsPoolId);
            }

            if (selectedParams.CmsLocationGroupId != null)
            {
                returned = returned.Where(d => d.LOCATION.cms_location_group_id == selectedParams.CmsLocationGroupId);
            }

            if (selectedParams.OpsAreadId != null)
            {
                returned = returned.Where(d => d.LOCATION.ops_area_id == selectedParams.OpsAreadId);
            }

            if (selectedParams.OpsRegionId != null)
            {
                returned = returned.Where(d => d.LOCATION.OPS_AREA.ops_region_id == selectedParams.OpsRegionId);
            }

            if (selectedParams.Branch != null)
            {
                returned = returned.Where(d => d.LocId == int.Parse(selectedParams.Branch));
            }

            if (selectedParams.SegmentId != null)
            {
                returned = returned.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == selectedParams.SegmentId);
            }

            if (selectedParams.ClassId != null)
            {
                returned = returned.Where(d => d.CAR_GROUP.car_class_id == selectedParams.ClassId);
            }

            if (selectedParams.GroupId != null)
            {
                returned = returned.Where(d => d.CarGrpId == selectedParams.GroupId);
            }

            return returned;
        }

        public List<AdditionDeletionGridViewHolder> GetDataGridAdditions(ReportsParameters selectedParams, EnumAdditionDeletion type)
        {
            var additions = from ra in GetAdditons(selectedParams)
                select new AdditionDeletionGridViewHolder
                       {
                           Identifier = ra.Id,
                           LocationWwd = ra.LOCATION.location1,
                           LocationWwdId = ra.LocId,
                           CarGroup = ra.CAR_GROUP.car_group1,
                           CarGroupId = ra.CarGrpId,
                           RepDate = ra.RepDate,
                           Value = ra.Value,
                           Addition = true,
                       };
            var deletions = from ra in GetDeletions(selectedParams)
                            select new AdditionDeletionGridViewHolder
                            {
                                Identifier = ra.Id,
                                LocationWwd = ra.LOCATION.location1,
                                LocationWwdId = ra.LocId,
                                CarGroup = ra.CAR_GROUP.car_group1,
                                CarGroupId = ra.CarGrpId,
                                RepDate = ra.RepDate,
                                Value = ra.Value,
                                Addition = false
                            };

            
            switch (type)
            {
                case EnumAdditionDeletion.Both:
                    return additions.Union(deletions).ToList();
                    
                case EnumAdditionDeletion.Additions:
                    return additions.ToList();
                    
                case EnumAdditionDeletion.Deletions:
                    return deletions.ToList();
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public void ClearAdditionsAndDeletionsForCountry(string country)
        {
            var additionsToDelete = from ra in GetAdditionsForCountry(country)
                                select ra;
            _dataContext.ResAdditions.DeleteAllOnSubmit(additionsToDelete);

            var deletionsToDelete = from ra in GetDeletionsForCountry(country)
                                select ra;
            _dataContext.ResDeletions.DeleteAllOnSubmit(deletionsToDelete);

        }

        public void DeleteAdditionsAndDeletions(List<ResAddition> additions, List<ResDeletion> deletions)
        {
            _dataContext.ResAdditions.AttachAll(additions);
            _dataContext.ResDeletions.AttachAll(deletions);

            _dataContext.ResAdditions.DeleteAllOnSubmit(additions);
            _dataContext.ResDeletions.DeleteAllOnSubmit(deletions);
        }

        public void Submit()
        {
            //_dataContext.Log = new DebugTextWriter();
            _dataContext.SubmitChanges();
        }

        public void Dispose()
        {
            
            _dataContext.Dispose();
        }

        

        private IQueryable<ResAddition> GetAdditionsForCountry(string country)
        {
            var returned = from ra in _dataContext.ResAdditions
                           join cg in _dataContext.CAR_GROUPs on ra.CarGrpId equals cg.car_group_id
                           join cc in _dataContext.CAR_CLASSes on cg.car_class_id equals cc.car_class_id
                           join cs in _dataContext.CAR_SEGMENTs on cc.car_segment_id equals cs.car_segment_id
                           where cs.country == country
                           select ra;
            return returned;
        }

        private IQueryable<ResDeletion> GetDeletionsForCountry(string country)
        {
            var returned = from ra in _dataContext.ResDeletions
                           join cg in _dataContext.CAR_GROUPs on ra.CarGrpId equals cg.car_group_id
                           join cc in _dataContext.CAR_CLASSes on cg.car_class_id equals cc.car_class_id
                           join cs in _dataContext.CAR_SEGMENTs on cc.car_segment_id equals cs.car_segment_id
                           where cs.country == country
                           select ra;
            return returned;
        }
    }
}
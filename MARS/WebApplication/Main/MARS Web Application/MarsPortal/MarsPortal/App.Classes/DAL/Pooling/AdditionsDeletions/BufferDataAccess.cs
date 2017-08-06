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
    public class BufferDataAccess : IDisposable
    {
        private readonly MarsDBDataContext _dataContext;

        public BufferDataAccess(MarsDBDataContext dc = null)
        {
            _dataContext = dc ?? new MarsDBDataContext(MarsConnection.ConnectionString);
            
        }

        public LOCATION GetLocationId(string locationString)
        {
            var addDelDa = new AdditionDeletionDataAccess(_dataContext);

            var returned = addDelDa.GetLocationId(locationString);
            return returned;
        }

        public int GetCarGroupId(string country, string carGroup)
        {
            var addDelDa = new AdditionDeletionDataAccess(_dataContext);

            var returned = addDelDa.GetCarGroupId(country, carGroup);
            return returned;
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public void AttachBuffers(List<ResBuffer> rb)
        {
            _dataContext.ResBuffers.AttachAll(rb);
            _dataContext.Refresh(RefreshMode.KeepCurrentValues, rb);
        }

        public void InsertBuffer(ResBuffer rb)
        {
            _dataContext.ResBuffers.InsertOnSubmit(rb);
        }

        public void InsertBuffers(List<ResBuffer> ra)
        {
            _dataContext.ResBuffers.InsertAllOnSubmit(ra);
        }

        private IQueryable<ResBuffer> GetBuffersForCountry(string country)
        {
            var returned = from ra in _dataContext.ResBuffers
                           join cg in _dataContext.CAR_GROUPs on ra.CarGrpId equals cg.car_group_id
                           join cc in _dataContext.CAR_CLASSes on cg.car_class_id equals cc.car_class_id
                           join cs in _dataContext.CAR_SEGMENTs on cc.car_segment_id equals cs.car_segment_id
                           where cs.country == country
                           select ra;
            return returned;
        }

        private IQueryable<ResBuffer> GetBuffers(ReportsParameters selectedParams)
        {
            var returned = from ls in _dataContext.ResBuffers
                           select ls;

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



        public List<BufferGridViewHolder> GetDataGridAdditions(ReportsParameters reportParameters)
        {
            var buffers = from ra in GetBuffers(reportParameters)
                            select new BufferGridViewHolder
                            {
                                Identifier = ra.Id,
                                LocationWwd = ra.LOCATION.location1,
                                LocationWwdId = ra.LocId,
                                CarGroup = ra.CAR_GROUP.car_group1,
                                CarGroupId = ra.CarGrpId,
                                Value = ra.Value,
                            };

            return buffers.ToList();
        }

        public void ClearBuffersForCountry(string country)
        {
            var buffersToDelete = from ra in GetBuffersForCountry(country)
                                    select ra;
            _dataContext.ResBuffers.DeleteAllOnSubmit(buffersToDelete);
        }

        public void DeleteBuffers(List<ResBuffer> buffers)
        {
            _dataContext.ResBuffers.AttachAll(buffers);
            _dataContext.ResBuffers.DeleteAllOnSubmit(buffers);
        }

        public void Submit()
        {
            //_dataContext.Log = new DebugTextWriter();
            _dataContext.SubmitChanges();
        }
    }
}
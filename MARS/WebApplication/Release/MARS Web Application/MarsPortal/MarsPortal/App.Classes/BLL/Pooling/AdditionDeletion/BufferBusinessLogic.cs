using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.NonRevReWrite.ParameterHolders;
using Mars.App.Classes.DAL.Pooling.AdditionsDeletions;

namespace Mars.App.Classes.BLL.Pooling.AdditionDeletion
{
    public class BufferBusinessLogic
    {
        public BufferFileSummary ProcessFile(string country, HttpPostedFile file)
        {
            var returned = new BufferFileSummary();
            var fileData = new byte[file.ContentLength];
            file.InputStream.Read(fileData, 0, file.ContentLength);
            var rawBufferDataHolders = new List<RawBufferHolder>();


            using (var ms = new MemoryStream(fileData))
            {
                var sr = new StreamReader(ms);

                var headerLine = sr.ReadLine();
                if (string.IsNullOrEmpty(headerLine))
                {
                    returned.FilePaseMessage = "Invalid File";
                    return returned;
                }
                var columns = headerLine.Split(',');

                if (columns[0] != "Location")
                {
                    returned.FilePaseMessage = "Invalid Header";
                }

                while (!sr.EndOfStream)
                {
                    var dataRow = sr.ReadLine();
                    if (dataRow == null) continue;
                    var splitDataRow = dataRow.Split(',');

                    var rawData = new RawBufferHolder
                    {
                        Location = splitDataRow[0].Trim(),
                        CarGroup = splitDataRow[1].Trim(),
                        Number = splitDataRow[2].Trim(),
                    };
                    rawBufferDataHolders.Add(rawData);
                }
            }

            ParseRawData(rawBufferDataHolders, returned, country);
            return returned;
        }

        public List<BufferGridViewHolder> GetBufferGridViewHolders(ReportsParameters reportParams)
        {
            List<BufferGridViewHolder> returned;
            using (var dataAccess = new BufferDataAccess())
            {
                returned = dataAccess.GetDataGridAdditions(reportParams);
            }
            return returned;
        }

        public void UpdateBuffers(List<BufferGridViewHolder> adData)
        {
            var buffers = adData.Select(d => new ResBuffer
                                             {
                                                 Id = d.Identifier, CarGrpId = d.CarGroupId, LocId = d.LocationWwdId, Value = d.Value
                                             }).ToList();

            using (var dataAccess = new BufferDataAccess())
            {
                dataAccess.AttachBuffers(buffers);
                dataAccess.Submit();
            }
        }

        public string InsertManualBuffer(BufferGridViewHolder data)
        {
            using (var dataAccess = new BufferDataAccess())
            {
                var selectedLocation = dataAccess.GetLocationId(data.LocationWwd);
                if (selectedLocation == null)
                {
                    return "Invalid Locaiton Code Entered";
                }
                var carGroupId = dataAccess.GetCarGroupId(selectedLocation.country, data.CarGroup);
                if (carGroupId == 0)
                {
                    return "Invalid Car Group Entered";
                }

                var resBuffer = new ResBuffer
                {
                    LocId = selectedLocation.dim_Location_id,
                    CarGrpId = carGroupId,
                    Value = data.Value
                };
                dataAccess.InsertBuffer(resBuffer);
                dataAccess.Submit();
            }
            return "Entry Added";
        }

        private void ParseRawData(IEnumerable<RawBufferHolder> rawData, BufferFileSummary bfs, string country)
        {
            bfs.Buffers = new List<ResBuffer>();
            
            using (var dataAccess = new BufferDataAccess())
            {
                foreach (var rd in rawData)
                {
                    var location = dataAccess.GetLocationId(rd.Location);
                    if (location == null || location.country != country)
                    {
                        bfs.RowsSkipped++;
                        continue;
                    }

                    var groupId = dataAccess.GetCarGroupId(location.country, rd.CarGroup);
                    if (groupId == 0)
                    {
                        bfs.RowsSkipped++;
                        continue;
                    }
                    
                    int number;
                    var parseSucceeded = int.TryParse(rd.Number, out number);
                    if (!parseSucceeded)
                    {
                        bfs.RowsSkipped++;
                        continue;
                    }

                    
                    var rAdd = new ResBuffer
                    {
                        LocId = location.dim_Location_id,
                        CarGrpId = groupId,
                        Value = number
                    };
                    bfs.Buffers.Add(rAdd);
                    bfs.ValidRows++;
                    
                }
            }
        }

        public string InsertNewBuffers(List<ResBuffer> rb, string country)
        {
            using (var dataAccess = new BufferDataAccess())
            {
                try
                {
                    dataAccess.ClearBuffersForCountry(country);
                    dataAccess.InsertBuffers(rb);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                dataAccess.Submit();
            }

            return "Buffers Successfully Uploaded";
        }

        public void DeleteBuffers(List<BufferGridViewHolder> entitiesToDelete)
        {
            var rb = entitiesToDelete.Select(e => new ResBuffer
                                                  {
                                                      Id = e.Identifier, CarGrpId = e.CarGroupId, LocId = e.LocationWwdId, Value = e.Value
                                                  }).ToList();

            using (var dataAccess = new BufferDataAccess())
            {
                dataAccess.DeleteBuffers(rb);
                dataAccess.Submit();
            }
        }

    }
}
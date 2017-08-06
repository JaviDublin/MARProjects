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
    public class AdditionDeletionBusinessLogic
    {
        private const string DeletionIdentifier = "DEL";
        private const string AdditionIdentifier = "ADD";

        public AdditionDeletionFileSummary ProcessFile(string country, HttpPostedFile file)
        {
            var returned = new AdditionDeletionFileSummary();
            var fileData = new byte[file.ContentLength];
            file.InputStream.Read(fileData, 0, file.ContentLength);
            var rawStringAdditionDeletionDataHolders = new List<RawAdditionDeletionDataHolder>();


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
                    return returned;
                }


                while (!sr.EndOfStream)
                {
                    var dataRow = sr.ReadLine();
                    if (dataRow == null) continue;
                    var splitDataRow = dataRow.Split(',');
                    if (splitDataRow.Count() != 6)
                    {
                        continue;
                    }

                    var rawData = new RawAdditionDeletionDataHolder
                                     {
                                         Location = splitDataRow[0].Trim(),
                                         CarGroup = splitDataRow[1].Trim(),
                                         Day = splitDataRow[2].Trim(),
                                         Time = splitDataRow[3].Trim(),
                                         Number = splitDataRow[4].Trim(),
                                         AddDel = splitDataRow[5].Trim()
                                     };
                    rawStringAdditionDeletionDataHolders.Add(rawData);
                }
            }

            ParseRawData(rawStringAdditionDeletionDataHolders, returned, country);
            return returned;
        }


        public List<AdditionDeletionGridViewHolder> GetAdditionDeletionGridViewHolders(ReportsParameters selectedParams,EnumAdditionDeletion type)
        {
            List<AdditionDeletionGridViewHolder> returned;
            using (var dataAccess = new AdditionDeletionDataAccess())
            {
                returned = dataAccess.GetDataGridAdditions(selectedParams, type);
            }
            return returned;
        }

        public void UpdateAdditionDeletions(List<AdditionDeletionGridViewHolder> adData)
        {
            var additions = new List<ResAddition>();
            var deletions = new List<ResDeletion>();
            foreach (var d in adData)
            {
                if (d.Addition)
                {
                    additions.Add(new ResAddition
                                  {
                                      Id = d.Identifier,
                                      CarGrpId = d.CarGroupId,
                                      LocId = d.LocationWwdId,
                                      RepDate = d.RepDate,
                                      Value = d.Value
                                  });
                }
                else
                {
                    deletions.Add(new ResDeletion
                                {
                                    Id = d.Identifier,
                                    CarGrpId = d.CarGroupId,
                                    LocId = d.LocationWwdId,
                                    RepDate = d.RepDate,
                                    Value = d.Value
                                });
                }
            }
            using (var dataAccess = new AdditionDeletionDataAccess())
            {
                dataAccess.AttachAdditions(additions);
                dataAccess.AttachDeletions(deletions);
                dataAccess.Submit();
            }
        }

        public string InsertManualAdditionDeletion(AdditionDeletionGridViewHolder data)
        {
            using (var dataAccess = new AdditionDeletionDataAccess())
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

                if (data.Addition)
                {

                    var resAddition = new ResAddition
                                      {
                                          LocId = selectedLocation.dim_Location_id,
                                          CarGrpId = carGroupId,
                                          RepDate = data.RepDate,
                                          Value = data.Value
                                      };
                    dataAccess.InsertAddition(resAddition);
                }
                else
                {
                    var resDeletion = new ResDeletion
                    {
                        LocId = selectedLocation.dim_Location_id,
                        CarGrpId = carGroupId,
                        RepDate = data.RepDate,
                        Value = data.Value
                    };
                    dataAccess.InsertDeletion(resDeletion);
                }
                dataAccess.Submit();
            }
            return "Entry Added";
        }

        private void ParseRawData(IEnumerable<RawAdditionDeletionDataHolder> rawData, AdditionDeletionFileSummary adfs, string country)
        {
            adfs.Additions = new List<ResAddition>();
            adfs.Deletions = new List<ResDeletion>();
            Dictionary<string, int> validLocations;
            Dictionary<string, int> validCarGroups;
            using (var dataAccess = new AdditionDeletionDataAccess())
            {
                validLocations = dataAccess.GetValidLocationsForCountry(country);
                validCarGroups = dataAccess.GetValidCarGroups(country);
            }
                foreach (var rd in rawData)
                {
                    
                    if (!validLocations.ContainsKey(rd.Location))
                    {
                        adfs.RowsSkipped++;
                        continue;
                    }

                    //var groupId = dataAccess.GetCarGroupId(location.country, rd.CarGroup);
                    if (!validCarGroups.ContainsKey(rd.CarGroup))
                    {
                        adfs.RowsSkipped++;
                        continue;
                    }
                    DateTime repDate;

                    var parseSucceeded = DateTime.TryParse(string.Format("{0} {1}", rd.Day, rd.Time), out repDate);
                    if (!parseSucceeded)
                    {
                        adfs.RowsSkipped++;
                        continue;
                    }
                    int number;
                    parseSucceeded = int.TryParse(rd.Number, out number);
                    if (!parseSucceeded)
                    {
                        adfs.RowsSkipped++;
                        continue;
                    }

                    if (rd.AddDel == AdditionIdentifier)
                    {
                        var rAdd = new ResAddition
                                 {
                                     LocId = validLocations[rd.Location],
                                     CarGrpId = validCarGroups[rd.CarGroup],
                                     RepDate = repDate,
                                     Value = number
                                 };
                        adfs.Additions.Add(rAdd);
                        adfs.ValidRows++;
                    }
                    else if (rd.AddDel == DeletionIdentifier)
                    {
                        var rDel = new ResDeletion
                            {
                                LocId = validLocations[rd.Location],
                                CarGrpId = validCarGroups[rd.CarGroup],
                                RepDate = repDate,
                                Value = number
                            };
                        adfs.Deletions.Add(rDel);
                        adfs.ValidRows++;
                    }
                    else
                    {
                        adfs.RowsSkipped++;
                    }
                }
            
        }

        public string InsertNewAdditionsDeletions(List<ResAddition> ra, List<ResDeletion> rd, string country)
        {
            using (var dataAccess = new AdditionDeletionDataAccess())
            {
                try
                {
                    dataAccess.ClearAdditionsAndDeletionsForCountry(country);
                    dataAccess.InsertAdditions(ra);
                    dataAccess.InsertDeletions(rd);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                dataAccess.Submit();
            }

            return "Additions and Deletions Successfully Uploaded";
        }

        public void DeleteAdditionDeletions(List<AdditionDeletionGridViewHolder> entitiesToDelete)
        {
            var ra = new List<ResAddition>();
            var rd = new List<ResDeletion>();

            foreach (var e in entitiesToDelete)
            {
                if (e.Addition)
                {
                    ra.Add(new ResAddition
                           {
                               Id = e.Identifier,
                               CarGrpId = e.CarGroupId,
                               LocId = e.LocationWwdId,
                               RepDate = e.RepDate,
                               Value = e.Value
                           });
                }
                else
                {
                    rd.Add(new ResDeletion
                           {
                               Id = e.Identifier,
                               CarGrpId = e.CarGroupId,
                               LocId = e.LocationWwdId,
                               RepDate = e.RepDate,
                               Value = e.Value
                           });
                }
            }
            using (var dataAccess = new AdditionDeletionDataAccess())
            {
                dataAccess.DeleteAdditionsAndDeletions(ra, rd);
                dataAccess.Submit();
            }
        }
    }
}
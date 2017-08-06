using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using Mars.FleetAllocation.DataContext;


namespace Mars.FleetAllocation.DataAccess.ScenarioAccess
{
    internal class MinCommercialSegmentDataAccess : BaseDataAccess
    {
        internal List<ListItem> GetMinCommSegScenarios(int countryId)
        {
            var listItems = from scens in DataContext.MinCommercialSegmentScenarios
                            where scens.CountryId == countryId
                select new ListItem(scens.ScenarioName, scens.MinCommercialSegmentScenarioId.ToString());

            var returned = listItems.ToList();
            return returned;
        }

        internal string RenameScenario(int scenarioId, string newScenarioName)
        {
            var scenarioExists = CheckScenarioExists(newScenarioName);
            if (scenarioExists != string.Empty) return scenarioExists;

            var scenarioEntity = DataContext.MinCommercialSegmentScenarios.Single(d => d.MinCommercialSegmentScenarioId == scenarioId);

            scenarioEntity.ScenarioName = newScenarioName;
            DataContext.SubmitChanges();

            return string.Empty;
        }

        internal string CreateNewScenario(string newScenarioName, int countryId)
        {
            var scenarioExists = CheckScenarioExists(newScenarioName);
            if (scenarioExists != string.Empty) return scenarioExists;

            var scenario = new MinCommercialSegmentScenario
                           {
                               ScenarioName = newScenarioName,
                               CountryId = countryId
                           };

            CreateScenario(scenario);

            return string.Empty;
        }

        private string CheckScenarioExists(string scenarioName)
        {
            return DataContext.MinCommercialSegmentScenarios.Any(d => d.ScenarioName == scenarioName) ? "This scenario name already exists" : string.Empty;
        }

        private void CreateScenario(MinCommercialSegmentScenario scenarioToCreate)
        {
            DataContext.MinCommercialSegmentScenarios.InsertOnSubmit(scenarioToCreate);
            DataContext.SubmitChanges();
        }

        internal string CloneExistingScenario(int existingScenarioId, string newScenarioName, int countryId)
        {
            var scenarioExists = CheckScenarioExists(newScenarioName);
            if (scenarioExists != string.Empty) return scenarioExists;

            var scenario = new MinCommercialSegmentScenario
                {
                    ScenarioName = newScenarioName,
                    CountryId = countryId
                };

            CreateScenario(scenario);

            var newScenarioId = scenario.MinCommercialSegmentScenarioId;

            DataContext.DuplicateMinCommSeg(newScenarioId, existingScenarioId);
            
            return string.Empty;
        }

        internal string DeleteScenario(int scenarioId)
        {
            var scenarioEntriesToDelete =
                DataContext.MinCommercialSegments.Where(d => d.MinCommercialSegmentScenarioId == scenarioId);

            if (scenarioEntriesToDelete.Any())
            {
                DataContext.MinCommercialSegments.DeleteAllOnSubmit(scenarioEntriesToDelete);

                DataContext.SubmitChanges();
            }
        
            var scenarioToDelete =
                DataContext.MinCommercialSegmentScenarios.Single(d => d.MinCommercialSegmentScenarioId == scenarioId);

            DataContext.MinCommercialSegmentScenarios.DeleteOnSubmit(scenarioToDelete);

            DataContext.SubmitChanges();
            return string.Empty;
        }

        internal string GetScenarioDescription(int scenarioId)
        {
            var returned =
                DataContext.MinCommercialSegmentScenarios.Single(d => d.MinCommercialSegmentScenarioId == scenarioId)
                    .Description;
            return returned;
        }

        internal void SetScenarioDescription(int scenarioId, string description)
        {
            var scenarioEntity =
                DataContext.MinCommercialSegmentScenarios.Single(d => d.MinCommercialSegmentScenarioId == scenarioId);
            scenarioEntity.Description = description;
            DataContext.SubmitChanges();
        }

        internal int CountScenarioEntries(int sccenarioId)
        {
            var scenarioEntities =
                DataContext.MinCommercialSegments.Count(d => d.MinCommercialSegmentScenarioId == sccenarioId);

            return scenarioEntities;
        }
    }
}
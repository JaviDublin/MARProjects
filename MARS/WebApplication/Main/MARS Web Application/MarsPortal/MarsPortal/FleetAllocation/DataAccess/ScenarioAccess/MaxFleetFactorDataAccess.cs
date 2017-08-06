using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.ScenarioAccess
{
    public class MaxFleetFactorDataAccess : BaseDataAccess
    {
        internal List<ListItem> GetMaxFleetFactorScenarios(int countryId)
        {
            var listItems = from scens in DataContext.MaxFleetFactorScenarios
                            where scens.CountryId == countryId
                            select new ListItem(scens.ScenarioName, scens.MaxFleetFactorScenarioId.ToString());

            var returned = listItems.ToList();
            return returned;
        }

        internal string RenameScenario(int scenarioId, string newScenarioName)
        {
            var scenarioExists = CheckScenarioExists(newScenarioName);
            if (scenarioExists != string.Empty) return scenarioExists;

            var scenarioEntity = DataContext.MaxFleetFactorScenarios.Single(d => d.MaxFleetFactorScenarioId == scenarioId);

            scenarioEntity.ScenarioName = newScenarioName;
            DataContext.SubmitChanges();

            return string.Empty;
        }

        internal string CreateNewScenario(string newScenarioName, int countryId)
        {
            var scenarioExists = CheckScenarioExists(newScenarioName);
            if (scenarioExists != string.Empty) return scenarioExists;

            var scenario = new MaxFleetFactorScenario
            {
                ScenarioName = newScenarioName,
                CountryId = countryId
            };

            CreateScenario(scenario);

            return string.Empty;
        }

        private string CheckScenarioExists(string scenarioName)
        {
            return DataContext.MaxFleetFactorScenarios.Any(d => d.ScenarioName == scenarioName) ? "This scenario name already exists" : string.Empty;
        }

        private void CreateScenario(MaxFleetFactorScenario scenarioToCreate)
        {
            DataContext.MaxFleetFactorScenarios.InsertOnSubmit(scenarioToCreate);
            DataContext.SubmitChanges();
        }

        internal string CloneExistingScenario(int existingScenarioId, string newScenarioName, int countryId)
        {
            var scenarioExists = CheckScenarioExists(newScenarioName);
            if (scenarioExists != string.Empty) return scenarioExists;

            var scenario = new MaxFleetFactorScenario
            {
                ScenarioName = newScenarioName,
                CountryId = countryId
            };

            CreateScenario(scenario);

            var newScenarioId = scenario.MaxFleetFactorScenarioId;

            DataContext.DuplicateMaxFleet(newScenarioId, existingScenarioId);

            return string.Empty;
        }

        internal string DeleteScenario(int scenarioId)
        {
            var scenarioEntriesToDelete =
                DataContext.MaxFleetFactors.Where(d => d.MaxFleetFactorScenarioId == scenarioId);

            DataContext.MaxFleetFactors.DeleteAllOnSubmit(scenarioEntriesToDelete);

            var scenarioToDelete =
                DataContext.MaxFleetFactorScenarios.Single(d => d.MaxFleetFactorScenarioId == scenarioId);

            DataContext.MaxFleetFactorScenarios.DeleteOnSubmit(scenarioToDelete);

            DataContext.SubmitChanges();
            return string.Empty;
        }

        internal string GetScenarioDescription(int scenarioId)
        {
            var returned =
                DataContext.MaxFleetFactorScenarios.Single(d => d.MaxFleetFactorScenarioId == scenarioId)
                    .Description;
            return returned;
        }

        internal void SetScenarioDescription(int scenarioId, string description)
        {
            var scenarioEntity =
                DataContext.MaxFleetFactorScenarios.Single(d => d.MaxFleetFactorScenarioId == scenarioId);
            scenarioEntity.Description = description;
            DataContext.SubmitChanges();
        }


        internal int CountScenarioEntries(int sccenarioId)
        {
            var scenarioEntities =
                DataContext.MaxFleetFactors.Count(d => d.MaxFleetFactorScenarioId == sccenarioId);

            return scenarioEntities;
        }

    }
}
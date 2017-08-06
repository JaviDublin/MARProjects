using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;
using System.Data.Linq;

namespace App.DAL.VehiclesAbroad.Queryables {
    public class VehicleDetailsSort_Switch_Queryable {

        public IQueryable<ICarSearchDataEntity> getQueryable(string sortExpression, DataContext db, IQueryable<ICarSearchDataEntity> q) {
            switch (sortExpression) {
                case "Lstwwd": return q.OrderBy(p => p.Lstwwd);
                case "Lstwwd DESC": return q.OrderByDescending(p => p.Lstwwd);
                case "Lstdate": return q.OrderBy(p => p.Lstdate);
                //case "Lstdate DESC": return q.OrderByDescending(p => p.Lstdate);
                case "Vc": return q.OrderBy(p => p.Vc);
                case "Vc DESC": return q.OrderByDescending(p => p.Vc);
                case "Unit": return q.OrderBy(p => p.Unit);
                case "Unit DESC": return q.OrderByDescending(p => p.Unit);
                case "License": return q.OrderBy(p => p.License);
                case "License DESC": return q.OrderByDescending(p => p.License);
                case "Model": return q.OrderBy(p => p.Model);
                case "Model DESC": return q.OrderByDescending(p => p.Model);
                case "Moddesc": return q.OrderBy(p => p.Moddesc);
                case "Moddesc DESC": return q.OrderByDescending(p => p.Moddesc);
                case "Duewwd": return q.OrderBy(p => p.Duewwd);
                case "Duewwd DESC": return q.OrderByDescending(p => p.Duewwd);
                //case "Duedate": return q.OrderBy(p => p.Duedate);
                //case "Duedate DESC": return q.OrderByDescending(p => p.Duedate);
                //case "Duetime": return q.OrderBy(p => p.Duetime);
                //case "Duetime DESC": return q.OrderByDescending(p => p.Duetime);
                case "Op": return q.OrderBy(p => p.Op);
                case "Op DESC": return q.OrderByDescending(p => p.Op);
                case "Mt": return q.OrderBy(p => p.Mt);
                case "Mt DESC": return q.OrderByDescending(p => p.Mt);
                case "Nr": return q.OrderBy(p => p.Nonrev);
                case "Nr DESC": return q.OrderByDescending(p => p.Nonrev);
                case "Driver": return q.OrderBy(p => p.Driver);
                case "Driver DESC": return q.OrderByDescending(p => p.Driver);
                case "Doc": return q.OrderBy(p => p.Doc);
                case "Doc DESC": return q.OrderByDescending(p => p.Doc);
                case "Lstmlg": return q.OrderBy(p => p.Lstmlg);
                case "Lstmlg DESC": return q.OrderByDescending(p => p.Lstmlg);
                case "NonRev": return q.OrderBy(p => p.Nonrev);
                case "NonRev DESC": return q.OrderByDescending(p => p.Nonrev);
                default: return q;
            }
        }
    }
}
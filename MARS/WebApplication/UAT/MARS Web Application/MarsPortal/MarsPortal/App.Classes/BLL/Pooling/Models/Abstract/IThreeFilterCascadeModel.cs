using App.BLL.VehiclesAbroad.Models.Abstract;
using Mars.DAL.Pooling.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.Pooling.Models.Abstract;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IThreeFilterCascadeModel {
        IFilterModel3 TopModel { get; set; }
        IFilterModel3 BottomModel { get; set; }
        IFilterModel3 MiddleModel { get; set; }
        void bind(params string[] dependants);
        void SuperSelected(params string[] dependants);
        void TopSelected(params string[] dependants);
        void MiddleSelected(params string[] dependants);
        void rebind(int it,int im,int ib,string st);
        void CascadeRebind(int it, int im, int ib, string st);
        void SetRepositories(IFilterRepository3 t,IFilterRepository3 m,IFilterRepository3 b);
        void SetRepositories(IThreeFilterRepository r);
    }
}

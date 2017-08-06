using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace App.BLL
{
    public enum MenuType : int
    {
        MenuMultiViewA = 1,
        MenuMultiViewB = 2,
        MenuMultiViewC = 3,
        MenuMultiViewD = 4,
        MenuMultiViewE = 5
    };

    public class Navigation
    {
        public int Index { get; set; }

        public Navigation(int index)
        {
            Index = index;
        }
    }

    public class NavigationMenu : IComparable<NavigationMenu>
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string ToolTip { get; set; }

        int IComparable<NavigationMenu>.CompareTo(NavigationMenu compare)
        {
            if (compare.Index > this.Index)
                return -1;
            else if (compare.Index == this.Index)
                return 0;
            else
                return 1;
        }

        public static int GetMinimumIndex(List<NavigationMenu> navigationmenu)
        {
            NavigationMenu result = navigationmenu.Min();
            return result.Index;
        }

        public NavigationMenu(int index, string title, string toolTip, string imageUrl)
        {
            Index = index;
            if (imageUrl != null)
            {
                Title = @"<img src='" + imageUrl + "' class='image-SideMenu'> " + title;
            }
            else
            {
                Title = title;
            }

            ToolTip = toolTip;

        }

        public static List<NavigationMenu> SelectMenuItem(int selectedMenu, Page selectedPage)
        {
            var results = new List<NavigationMenu>();
            
            switch (selectedMenu)
            {
                case (int)MenuType.MenuMultiViewA:
                    results.Add(new NavigationMenu(0, "List", "Click to View the List", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_list.png")));
                    results.Add(new NavigationMenu(1, "Form", "Click to View the Form", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_form.png")));
                    results.Add(new NavigationMenu(2, "History", "Click to View the History", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_list.png")));
                    break;
                case (int)MenuType.MenuMultiViewB:
                    results.Add(new NavigationMenu(0, "General", "Click to View the General Filters", selectedPage.ResolveUrl("~/App.Images/Icons/magnifier.png")));
                    results.Add(new NavigationMenu(1, "Vehicle", "Click to View the Vehicle Filters", selectedPage.ResolveUrl("~/App.Images/Icons/magnifier.png")));
                    break;
                case (int)MenuType.MenuMultiViewC:
                    results.Add(new NavigationMenu(0, "Remarks", "Click to View the Remark List", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_list.png")));
                    results.Add(new NavigationMenu(1, "Ageing", "Click to View the Ageing List", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_list.png")));
                    results.Add(new NavigationMenu(2, "Chart", "Click to View the Chart", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_chart.png")));
                    break;
                case (int)MenuType.MenuMultiViewD:
                    results.Add(new NavigationMenu(0, "List", "Click to View the List", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_list.png")));
                    results.Add(new NavigationMenu(1, "Chart", "Click to View the Chart", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_chart.png")));
                    break;
                case (int)MenuType.MenuMultiViewE:
                    results.Add(new NavigationMenu(0, "Approval", "Click to View the Approval List", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_list.png")));
                    results.Add(new NavigationMenu(1, "History", "Click to View the Approval History", selectedPage.ResolveUrl("~/App.Images/Icons/multiview_list.png")));
                    break;
            }

            GenericLists.SortList<NavigationMenu, int>(results, x => x.Index);
            
            return results;
        }
    }

    public static class GenericLists
    {
        public static void SortList<TSource, TValue>(this List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(x), selector(y)));
        }

        public static void SortListDescending<TSource, TValue>(this List<TSource> source, Func<TSource, TValue> selector)
        {
            var comparer = Comparer<TValue>.Default;
            source.Sort((x, y) => comparer.Compare(selector(y), selector(x)));
        }
    }
}
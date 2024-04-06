using BookStoreManager.Database;
using BookStoreManager.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookStoreManager.Process
{
    public class HomePageBus
    {
        RevenueDao revenueDao = new RevenueDao();

        int curMonth, curYear, lstMonth, lstYear;

        public HomePageBus()
        {
            curMonth = DateTime.Now.Month;
            curYear = DateTime.Now.Year;

            if (curMonth != 1)
            {
                lstMonth = curMonth - 1;
                lstYear = curYear;
            }
            else
            {
                lstMonth = 12;
                lstYear = curYear - 1;
            }
        }

        public string CompareOrder()
        {
            string OrderState;

            int[] temps = new int[2];

            foreach (var data in revenueDao.GetRevenuesByMonth())
            {
                if (data.Month == curMonth && data.Year == curYear)
                    temps[0] += data.Quantity;
                if (data.Month == lstMonth && data.Year == lstYear)
                    temps[1] += data.Quantity;
            }

            if (((double)(temps[0] - temps[1]) / temps[1] * 100) >= 0)
                OrderState = "tăng";
            else
                OrderState = "giảm";

            string result = $"Tổng đơn hàng tháng {curMonth}/{curYear} {OrderState} {Math.Abs((double)(temps[0] - temps[1]) / temps[1] * 100).ToString("0")}% so với tháng {lstMonth}/{lstYear}";

            return result;
        }

        public string CompareRevenue()
        {
            string RevenueState;

            int[] temps = new int[2];

            foreach (var data in revenueDao.GetRevenuesByMonth())
            {
                if (data.Month == curMonth && data.Year == curYear)
                    temps[0] += data.Revenue;
                if (data.Month == lstMonth && data.Year == lstYear)
                    temps[1] += data.Revenue;
            }

            if (((double)(temps[0] - temps[1]) / temps[1] * 100) >= 0)
                RevenueState = "tăng";
            else
                RevenueState = "giảm";

            string result = $"Tổng doanh thu tháng {curMonth}/{curYear} {RevenueState} {Math.Abs((double)(temps[0] - temps[1]) / temps[1] * 100).ToString("0")}% so với tháng {lstMonth}/{lstYear}";

            return result;
        }
    }
}

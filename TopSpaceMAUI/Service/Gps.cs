using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Savage.Measurements;
using TopSpaceMAUI.Util;


//using Plugin.Geolocator;

namespace TopSpaceMAUI.Service
{
    public class Gps
    {
        // Current Position
        public Model.Gps CurrentPosition()
        {
            Util.Location utilLocation = new Util.Location();
            utilLocation.GetLocation();

            Model.Gps item = new Model.Gps();
            if (utilLocation.position != null)
            {
                item.Latitude = utilLocation.position.Latitude;
                item.Longitude = utilLocation.position.Longitude;
                if (utilLocation.position.Accuracy != null)
                    item.Precision = (double)utilLocation.position.Accuracy;
            }

            return item;

        }


        //Update list with Gps Position
        public List<Model.POS> POSListPositionUpdate(List<Model.POS> lstPOS, int limit, bool debug)
        {



            Service.Gps gpsService = new Gps();
            Model.Gps devicePosition = new Model.Gps();
            devicePosition = gpsService.CurrentPosition();


            if (debug == true)
            {
                             
                devicePosition.Latitude = -23.6357872;
                devicePosition.Longitude = -46.7903028;
                devicePosition.Precision = 5;

            }

         
            //update gps postion

            List<Model.POS> lstNewOrder = new List<Model.POS>();

            for (int i = 0; i <= (lstPOS.Count - 1); i++)            {


                if (lstPOS[i].Latitude != 0 && lstPOS[i].Longitude != 0 )
                {
                 
                     const double EarthRadius = 6371;
                   
                        double distance = 0;
                        double Lat = ((double)lstPOS[i].Latitude - devicePosition.Latitude) * (Math.PI / 180);
                        double Lon = ((double)lstPOS[i].Longitude - devicePosition.Longitude) * (Math.PI / 180);
                        double a = Math.Sin(Lat / 2) * Math.Sin(Lat / 2) + Math.Cos(devicePosition.Latitude * (Math.PI / 180)) * Math.Cos((double)lstPOS[i].Latitude * (Math.PI / 180)) * Math.Sin(Lon / 2) * Math.Sin(Lon / 2);
                        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                        distance = EarthRadius * c;
                        lstPOS[i].Distance = (decimal)distance;
                   

                    if (lstPOS[i].Distance < limit)
                    {
                        lstNewOrder.Add(lstPOS[i]);
                    }


                }
            }


            gpsService = null;


                return lstNewOrder.OrderBy(x =>x.Distance).ToList();

            }


            //get list Neighborhood
            public List<Model.POS> MyGPS(List<Model.POS> lstPOS, int limit, bool debug = false)
            {
                Model.Gps position = new Model.Gps();

                var result = POSListPositionUpdate(lstPOS, limit, debug).ToList();

                return result;

            }



    }


    
}

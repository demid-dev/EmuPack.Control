using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Models.Responses
{
    public class StatusCommandResponse : CommandResponse
    {
        public string DrawerStatus { get; private set; }
        public string AdaptorStatus { get; private set; }
        public string RegistredPrescriptionsQuantity { get; private set; }
        public List<string> RegistredPrescriptionsIds { get; private set; }
        public string WarningCassettesQuantity { get; private set; }
        public List<string> WarningCassettesIds { get; private set; }

        public StatusCommandResponse(string response) :
            base(response)
        {
            DrawerStatus = response.Substring(StatusCommandResponseValues.DrawerStatusStartIndex,
                StatusCommandResponseValues.DrawerStatusLength);
            AdaptorStatus = response.Substring(StatusCommandResponseValues.AdaptorStartIndex,
                StatusCommandResponseValues.AdaptorLength);

            RegistredPrescriptionsQuantity = response
                .Substring(StatusCommandResponseValues.RegistredPrescriptionsQuantityStartIndex,
                    StatusCommandResponseValues.RegistredPrescriptionsQuantityLength);
            RegistredPrescriptionsIds = new List<string>();
            int prescriptionsQuantity = int.Parse(RegistredPrescriptionsQuantity);
            int warningCassettesQuantityStartIndex = StatusCommandResponseValues.RegistredPrescriptionsIdsStartIndex;
            for (int i = 0; i < prescriptionsQuantity; i++)
            {
                string presriptionId = response
                    .Substring(StatusCommandResponseValues.RegistredPrescriptionsIdsStartIndex
                    + i * StatusCommandResponseValues.RegistredPrescriptionsIdsLength,
                    StatusCommandResponseValues.RegistredPrescriptionsIdsLength);
                RegistredPrescriptionsIds.Add(GetNumberWithoutPadding(presriptionId));

                warningCassettesQuantityStartIndex += StatusCommandResponseValues.RegistredPrescriptionsIdsLength;
            }

            WarningCassettesQuantity = response.Substring(warningCassettesQuantityStartIndex,
                StatusCommandResponseValues.WarningCassettesQuantityLength);
            WarningCassettesIds = new List<string>();
            int cassettesQuantity = int.Parse(WarningCassettesQuantity);
            for (int i = 0; i < cassettesQuantity; i++)
            {
                string warningCassette = response
                    .Substring(warningCassettesQuantityStartIndex
                    + StatusCommandResponseValues.WarningCassettesQuantityLength
                    + i * StatusCommandResponseValues.WarningCassettesIdsLength,
                    StatusCommandResponseValues.WarningCassettesIdsLength);
                WarningCassettesIds.Add(warningCassette);
            }
        }
    }

    public static class StatusCommandResponseValues
    {
        static public int DrawerStatusStartIndex { get; private set; }
        static public int DrawerStatusLength { get; private set; }
        static public int AdaptorStartIndex { get; private set; }
        static public int AdaptorLength { get; private set; }
        static public int RegistredPrescriptionsQuantityStartIndex { get; private set; }
        static public int RegistredPrescriptionsQuantityLength { get; private set; }
        static public int RegistredPrescriptionsIdsStartIndex { get; private set; }
        static public int RegistredPrescriptionsIdsLength { get; private set; }
        static public int WarningCassettesQuantityLength { get; private set; }
        static public int WarningCassettesIdsLength { get; private set; }
        static public string DrawerOpenedValue { get; private set; }
        static public string AdaptorInDrawerValue { get; private set; }

        static StatusCommandResponseValues()
        {
            DrawerStatusStartIndex = 13;
            DrawerStatusLength = 1;
            AdaptorStartIndex = 14;
            AdaptorLength = 1;
            RegistredPrescriptionsQuantityStartIndex = 15;
            RegistredPrescriptionsQuantityLength = 2;
            RegistredPrescriptionsIdsStartIndex = 17;
            RegistredPrescriptionsIdsLength = 4;
            WarningCassettesQuantityLength = 2;
            WarningCassettesIdsLength = 2;
            DrawerOpenedValue = "1";
            AdaptorInDrawerValue = "1";
        }
    }
}

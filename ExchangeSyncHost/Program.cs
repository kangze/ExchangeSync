using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ExchangeSyncHost.Internal;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeSyncHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
            // 指定用户名，密码，和域名
            service.Url = new Uri("https://mail.scrbg.com/EWS/Exchange.asmx");
            service.Credentials = new WebCredentials("v-ms-kz@scrbg.com", "tfs4418000");
            // 指定Exchage服务的url地址
            MailManager.ReciveMailMessage(service, "v-ms-kz@scrbg.com");
            //var s = SyncMail(service, null);
            //SyncMail(service, s);
        }

        public static string SyncMail(ExchangeService service, string state)
        {
            ChangeCollection<FolderChange> fcc = service.SyncFolderHierarchy(new FolderId(WellKnownFolderName.Root), PropertySet.IdOnly, state);
            // If the count of changes is zero, there are no changes to synchronize.
            if (fcc.Count == 0)
            {
                Console.WriteLine("There are no folders to synchronize.");
            }
            // Otherwise, write all the changes included in the response 
            // to the console. 
            // For the initial synchronization, all the changes will be of type
            // ChangeType.Create.
            else
            {
                foreach (FolderChange fc in fcc)
                {
                    Console.WriteLine("ChangeType: " + fc.ChangeType.ToString());
                    Console.WriteLine("FolderId: " + fc.FolderId);
                    Console.WriteLine("===========");
                }
            }
            // Save the sync state for use in future SyncFolderItems requests.
            // The sync state is used by the server to determine what changes to report
            // to the client.
            string fSyncState = fcc.SyncState;
            return fSyncState;
        }


        public static void GetMail(ExchangeService service)
        {

            //FindItemsResults<Item> findResults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(int.MaxValue));
            //foreach (Item item in findResults.Items)
            //{
            //    item.Load();
            //    Console.WriteLine(item.Body.Text);
            //}

            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(30);
            const int NUM_APPTS = 5;

            // Initialize the calendar folder object with only the folder ID. 
            CalendarFolder calendar = CalendarFolder.Bind(service, WellKnownFolderName.Calendar, new PropertySet());

            // Set the start and end time and number of appointments to retrieve.
            CalendarView cView = new CalendarView(startDate, endDate, NUM_APPTS);

            // Limit the properties returned to the appointment's subject, start time, and end time.
            cView.PropertySet = new PropertySet(AppointmentSchema.Subject, AppointmentSchema.Start, AppointmentSchema.End);

            // Retrieve a collection of appointments by using the calendar view.
            FindItemsResults<Appointment> appointments = calendar.FindAppointments(cView);

            foreach (var appointmentsItem in appointments.Items)
            {
                appointmentsItem.Load();
            }

            string str = "\nThe first " + NUM_APPTS + " appointments on your calendar from " + startDate.Date.ToShortDateString() +
                         " to " + endDate.Date.ToShortDateString() + " are: \n";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace ExchangeSyncHost.Internal
{
    public static class MailManager
    {
        public static string ReciveMailMessage(ExchangeService service, string mailAddress)
        {
            Mailbox mb = new Mailbox(mailAddress);

            //creates a folder object that will point to inbox folder
            FolderId fid = new FolderId(WellKnownFolderName.Inbox, mb);

            //this will bind the mailbox you're looking for using your service instance
            Folder inbox = Folder.Bind(service, fid);

            //load items from mailbox inbox folder
            if (inbox != null)
            {
                FindItemsResults<Item> items = inbox.FindItems(new ItemView(100));

                foreach (var item in items)
                    Console.WriteLine(item);
            }

            return ":";
        }
    }
}

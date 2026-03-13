using Domain.Entity.BankModule;
using Microsoft.AspNetCore.SignalR;
using ServicesAbstraction;
using Domain.Entity.BankModule;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Services
{
    public class TransactionHub : Hub
    {
         
         
        public async Task JoinGroup(string bankId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, bankId);
        }

   
        public async Task BroadcastTransaction(Transactions transactions)
        {
             
            await Clients.All.SendAsync("NewTransaction", transactions);
        }

         
        public override async Task OnConnectedAsync()
        {
            

            await base.OnConnectedAsync();
        }
    }
}
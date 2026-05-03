using AutoMapper;
using Domain.Contract.Repositories;
using Domain.Entity.BankModule;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Services.Specificatinos;
using Shared.CardBankDto;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace Services.testing
{
    [TestFixture]
     public class CardBankServicesTesting
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapper;
        private Mock<UserManager<ApplicationUser>> usermanger;
        private Mock<IRepository<CardBank,string>> repo ;
        [SetUp]
        public void SetUp()
        {
             unitOfWorkMock = new Mock<IUnitOfWork>();
             mapper = new Mock<IMapper>();
             usermanger = MockUserManager();
            repo = new Mock<IRepository<CardBank, string>>();

        }

        [Test]

        public async  Task CreateCardTestingWithFauiler()
        {
             usermanger
                .Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser)null);

            var service = new CardBankServices(
                unitOfWorkMock.Object,
                mapper.Object,
                usermanger.Object
            );

            // Act
            var result = await service.CreateCardAsync("1", new CreatBankDto());

            // Assert
            Assert.  IsFalse(result.IsSuccess);
        }

        [Test]
        public async Task CreateCardBankWithSucces()
        {
               usermanger.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(new ApplicationUser { Id = "1", UserName = "YouseefSayedMohamed" });

            mapper.Setup(x => x.Map<CardBank>(It.IsAny<CreatBankDto>())).Returns(new CardBank { Name= "Visa",Amount=1000});

               
 
               
            var repo = new Mock<IRepository<CardBank, string>>();
            unitOfWorkMock.Setup(x => x.GetRepo<CardBank, string>()).Returns(repo.Object);
            unitOfWorkMock.Setup(x => x.SaveChangeRepoAsync()).ReturnsAsync(1);

            var services = new CardBankServices(
                unitOfWorkMock.Object,
                mapper.Object,
                usermanger.Object
                );


            var result = await services.CreateCardAsync("1", new CreatBankDto ());
          
            var data = result.Value;
             
            Assert.AreEqual("Visa", data.Name);
            ;

        }



        [Test]
        public async Task GetAllCardBank()
        {


            var fakeCards= new List<CardBank>()
            {
                new CardBank{ Id="13453463",Amount=10000,BankId=1,Name="Visa_1"},
                new CardBank{ Id="14363563",Amount=10200,BankId=2,Name="Visa_2"},
                new CardBank{ Id="13123463",Amount=14300,BankId=3,Name="Visa_3"}

            };
            var repo = new Mock<IRepository<CardBank, string>>();

            repo.Setup(x => x.GetAllSpecificationAsync(It.IsAny<CardBankSpecification>())).ReturnsAsync(fakeCards);
            unitOfWorkMock.Setup(x=>x.GetRepo<CardBank,string>()).Returns(repo.Object);

            var fakeDto = new List<CardDto>()
            {
                new CardDto{Amount=10000,BankId=1,Name="Visa_1"},
                new CardDto{Amount=10200,BankId=2,Name="Visa_2"},
                new CardDto{Amount=14300,BankId=3,Name="Visa_3"}
            };
            mapper.Setup(x => x.Map<IEnumerable<CardDto>>(It.IsAny<IEnumerable< CardBank>>())).Returns(fakeDto);
           
            var servier= new CardBankServices(unitOfWorkMock.Object,mapper.Object,usermanger.Object);

            var result= await servier.GetAllAsync();
            var data = result.Value;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, data.Count());
            
        }


        [Test]
        public async Task GetAllByID()
        {
            usermanger.Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync(new ApplicationUser { Id = "1", UserName = "AhemdSayed", PhoneNumber = "123456780", bankid = 1 });

            repo.Setup(x => x.GetAllSpecificationAsync(It.IsAny<CardBankSpecification>()));


            mapper.Setup(x=>x.Map<Card>)


        }
 








        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();

            return new Mock<UserManager<ApplicationUser>>(
                store.Object,
                null, // IOptions<IdentityOptions>
                null, // IPasswordHasher
                new List<IUserValidator<ApplicationUser>>(),
                new List<IPasswordValidator<ApplicationUser>>(),
                null, // ILookupNormalizer
                null, // IdentityErrorDescriber
                null, // IServiceProvider
                null  // ILogger
            );
        }
    }
}

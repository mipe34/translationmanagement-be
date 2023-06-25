using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Dal.Enums;
using TranslationManagement.Dal.Models;
using Xunit;

namespace TranslationManagement.Dal.Tests.Models
{
    public class TranslationJobTests
    {
        [Theory]
        [InlineData("", 0f, 0f)]
        [InlineData("12345", 2.2f, 11f)]
        public void SetPrice_CountsPriceFromOriginalLength(string originalContent, float pricePerCharacter, float expectedPrice)
        {
            var transJob = new TranslationJob() { OriginalContent = originalContent };
            transJob.SetPrice(pricePerCharacter);

            Assert.Equal(expectedPrice, transJob.Price);
        }

        [Fact]
        public void SetPrice_SetsPriceToZeroWhenPricePerCharacterIsNegative()
        {
            var transJob = new TranslationJob() { OriginalContent = "not empty" };
            transJob.SetPrice(-0.5f);

            Assert.Equal(0, transJob.Price);
        }

        [Theory]
        [InlineData(JobStatusEnum.New, JobStatusEnum.Inprogress)]
        [InlineData(JobStatusEnum.Inprogress, JobStatusEnum.Completed)]
        public void Set_StatusAllowedStatusesChange(JobStatusEnum current, JobStatusEnum newStatus)
        {
            var transJob = new TranslationJob() { Status = current };
            Assert.True(transJob.SetStatus(newStatus));
        }

        [Fact]
        public void SetStatus_CannotSkipInProggressStatus()
        {
            var transJob = new TranslationJob() { Status = Enums.JobStatusEnum.New };
            Assert.False(transJob.SetStatus(Enums.JobStatusEnum.Completed));
        }

        [Fact]
        public void SetStatus_CannotSetInitialStatus()
        {
            var transJob = new TranslationJob() { Status = Enums.JobStatusEnum.New };
            Assert.False(transJob.SetStatus(Enums.JobStatusEnum.New));
        }

        [Fact]
        public void SetStatus_CannotSetStatusWhenJobCompleted()
        {
            var transJob = new TranslationJob() { Status = Enums.JobStatusEnum.Completed };
            Assert.False(transJob.SetStatus(Enums.JobStatusEnum.Inprogress));
        }
    }

}

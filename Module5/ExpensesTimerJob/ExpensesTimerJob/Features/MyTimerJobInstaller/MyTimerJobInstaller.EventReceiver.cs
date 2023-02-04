using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace ExpensesTimerJob.Features.MyTimerJobInstaller
{
    /// <summary>
    /// Этот класс обрабатывает события, возникающие в ходе активации, деактивации, установки, удаления и обновления компонентов.
    /// </summary>
    /// <remarks>
    /// GUID, присоединенный к этому классу, может использоваться при создании пакета и не должен изменяться.
    /// </remarks>

    [Guid("423c4e76-6c3b-4d08-b67a-e4d945ca0c0d")]
    public class MyTimerJobInstallerEventReceiver : SPFeatureReceiver
    {
        const string timerJobName = "MyTimerJob";
        // Раскомментируйте ниже метод для обработки события, возникающего после активации компонента.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApplication = ((SPSite)properties.Feature.Parent).WebApplication;
            deleteJob(webApplication);
            MyTimerJob timerJob = new MyTimerJob(timerJobName, webApplication, null, SPJobLockType.Job);
            SPMinuteSchedule schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 1;
            schedule.EndSecond = 5;
            schedule.Interval = 45;
            timerJob.Schedule = schedule;
            timerJob.Update();

        }

        // Раскомментируйте ниже метод для обработки события, возникающего перед деактивацией компонента.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApplication = ((SPSite)properties.Feature.Parent).WebApplication;
            deleteJob(webApplication);
        }

        private void deleteJob(SPWebApplication webApplication)
        {
            foreach (SPJobDefinition job in webApplication.JobDefinitions)
            {
                if (job.Name.Equals(timerJobName))
                {
                    job.Delete();
                }
            }
        }

        // Раскомментируйте ниже метод для обработки события, возникающего после установки компонента.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Раскомментируйте ниже метод для обработки события, возникающего перед удалением компонента.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Раскомментируйте ниже метод для обработки события, возникающего при обновлении компонента.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}

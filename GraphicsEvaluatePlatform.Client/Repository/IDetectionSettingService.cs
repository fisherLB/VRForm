using GraphicsEvaluatePlatform.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsEvaluatePlatform.Client.Repository
{
    interface IDetectionSettingService
    {
        DetectionSetting GetDetail(Guid id);
    }
}

﻿using Amazon;

namespace BulutBusiness.Core.Translation;

public class AmazonTranslateConfiguration
{
    public string AccessKey { get; }
    public string SecretKey { get; }
    public RegionEndpoint RegionEndpoint { get; }

    public AmazonTranslateConfiguration(string accessKey, string secretKey, RegionEndpoint regionEndpoint)
    {
        AccessKey = accessKey;
        SecretKey = secretKey;
        RegionEndpoint = regionEndpoint;
    }
}

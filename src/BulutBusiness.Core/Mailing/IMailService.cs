﻿namespace BulutBusiness.Core.Mailing;

public interface IMailService
{
    void SendMail(Mail mail);
    Task SendEmailAsync(Mail mail);
}

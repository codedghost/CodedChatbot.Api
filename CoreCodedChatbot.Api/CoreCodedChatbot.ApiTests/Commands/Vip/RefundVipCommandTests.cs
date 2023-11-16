using System.Collections.Generic;
using AutoFixture.NUnit3;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using Moq;
using NUnit.Framework;

namespace CoreCodedChatbot.ApiTests.Commands.Vip;

[TestFixture]
public class RefundVipCommandTests
{
    private Mock<IRefundVipsRepository> _refundVipsRepository;

    private RefundVipCommand _subject;

    [SetUp]
    public void Setup()
    {
        _refundVipsRepository = new Mock<IRefundVipsRepository>();

        _subject = new RefundVipCommand(_refundVipsRepository.Object);
    }

    [Test, AutoData]
    public void EnsureSingleVipRefundCallsRepository(VipRefund vipRefund)
    {
        _subject.Refund(vipRefund);

        _refundVipsRepository.Verify(s => s.RefundVips(new List<VipRefund> {vipRefund}), Times.Once);
    }

    [Test, AutoData]
    public void EnsureMultipleVIpsRefundCallsRepository(List<VipRefund> vipRefunds)
    {
        _subject.Refund(vipRefunds);

        _refundVipsRepository.Verify(s => s.RefundVips(vipRefunds));
    }
}
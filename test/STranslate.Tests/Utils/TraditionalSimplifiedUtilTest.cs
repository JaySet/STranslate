using STranslate.Util;
using Xunit;

namespace STranslate.Tests.Utils;

public class TraditionalSimplifiedUtilTest
{
    [Fact]
    public void TestIsSimplified()
    {
        var traditional = "简体中文";
        var simplified = TraditionalSimplifiedUtil.IsSimplifiedChinese(traditional);
        Assert.True(simplified);
    }
    [Fact]
    public void TestIsSimplified2()
    {
        var traditional = "繁體中文";
        var simplified = TraditionalSimplifiedUtil.IsSimplifiedChinese(traditional);
        Assert.False(simplified);
    }
    
    [Fact]
    public void TestIsTraditional()
    {
        var traditional = "繁體中文";
        var simplified = TraditionalSimplifiedUtil.IsTraditionalChinese(traditional);
        Assert.True(simplified);
    }
    [Fact]
    public void TestIsTraditional2()
    {
        var traditional = "简体中文";
        var simplified = TraditionalSimplifiedUtil.IsTraditionalChinese(traditional);
        Assert.False(simplified);
    }
}
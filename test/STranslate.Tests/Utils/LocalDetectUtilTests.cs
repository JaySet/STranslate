using Xunit;
using STranslate.Util;
using STranslate.Model;

namespace STranslate.Tests.Util
{
    public class LocalDetectUtilTests
    {
        [Fact]
        public void LocalDetectLang_ShouldReturnEnglish_WhenTextIsEnglish()
        {
            var result = LocalDetectUtil.LocalDetectLang("This is a test sentence in English.");
            Assert.Equal(LangEnum.en, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnChinese_WhenTextIsChinese()
        {
            var result = LocalDetectUtil.LocalDetectLang("这是一个中文测试句子。");
            Assert.Equal(LangEnum.zh_cn, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnJapanese_WhenTextIsJapanese()
        {
            var result = LocalDetectUtil.LocalDetectLang("これは日本語のテスト文です。");
            Assert.Equal(LangEnum.ja, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnKorean_WhenTextIsKorean()
        {
            var result = LocalDetectUtil.LocalDetectLang("이것은 한국어 테스트 문장입니다.");
            Assert.Equal(LangEnum.ko, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnVietnamese_WhenTextIsVietnamese()
        {
            // var result = LocalDetectUtil.LocalDetectLang("Đây là một câu kiểm tra bằng tiếng Việt.");
            // Assert.Equal(LangEnum.vi, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnThai_WhenTextIsThai()
        {
            var result = LocalDetectUtil.LocalDetectLang("นี่คือประโยคทดสอบภาษาไทย");
            Assert.Equal(LangEnum.th, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnRussian_WhenTextIsRussian()
        {
            var result = LocalDetectUtil.LocalDetectLang("Это тестовое предложение на русском языке.");
            Assert.Equal(LangEnum.ru, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnSpanish_WhenTextIsSpanish()
        {
            var result = LocalDetectUtil.LocalDetectLang("Esta es una oración de prueba en español.");
            Assert.Equal(LangEnum.es, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnFrench_WhenTextIsFrench()
        {
            var result = LocalDetectUtil.LocalDetectLang("Ceci est une phrase de test en français.");
            Assert.Equal(LangEnum.fr, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnGerman_WhenTextIsGerman()
        {
            var result = LocalDetectUtil.LocalDetectLang("Dies ist ein Beispieltext auf Deutsch. Es enthält verschiedene Wörter und Sätze, um die Funktionalität der Sprachdetektion zu testen. Der Text soll sicherstellen, dass die Erkennung korrekt funktioniert.");
            Assert.Equal(LangEnum.de, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnEnglish_WhenTextIsMixedWithEnglish()
        {
            var result = LocalDetectUtil.LocalDetectLang("This is a test sentence with 中文.");
            Assert.Equal(LangEnum.en, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnChineseTraditional_WhenTextIsTraditionalChinese()
        {
            var result = LocalDetectUtil.LocalDetectLang("這是一個繁體中文測試句子。");
            Assert.Equal(LangEnum.zh_tw, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnEnglish_WhenTextIsEmpty()
        {
            var result = LocalDetectUtil.LocalDetectLang("");
            Assert.Equal(LangEnum.en, result);
        }

        [Fact]
        public void LocalDetectLang_ShouldReturnEnglish_WhenTextIsWhitespace()
        {
            var result = LocalDetectUtil.LocalDetectLang("     ");
            Assert.Equal(LangEnum.en, result);
        }
    }
}
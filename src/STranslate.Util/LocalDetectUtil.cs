using System.Text.RegularExpressions;
using STranslate.Model;

namespace STranslate.Util;

/// <summary>
///     <see href="https://github1s.com/openai-translator/openai-translator/blob/main/src/common/lang/index.ts"/>
/// </summary>
public class LocalDetectUtil
{
    public static LangEnum LocalDetectLang(string text)
    {
        if (text.Length > 200)
        {
            text = text[..200];
        }

        var langWeights = new Dictionary<string, int>
        {
            { "en", 0 },    // English
            { "zh", 0 },    // Chinese
            { "ko", 0 },    // Korean
            { "vi", 0 },    // Vietnamese
            { "th", 0 },    // Thai
            // { "hmn", 0 },    // Hmong
            { "ja", 0 },    // Japanese
            { "ru", 0 },    // Russian
            { "es", 0 },    // Spanish
            { "fr", 0 },    // French
            { "de", 0 },    // German
        };

        var vietnameseCharsRegEx = new Regex("[\u0103\u00E2\u00EA\u00F4\u01A1\u01B0\u1EA1\u1EB9\u1EC7\u1ED3\u1EDD\u1EF3\u1EA3\u1EBB\u1EC9\u1ED5\u1EDF\u1EF5\u1EA7\u1EBF\u1EC5\u1ED1\u1EDB\u1EF1\u1EA5\u1EBD\u1EC3\u1ECF\u1ED9\u1EE3\u1EF7\u1EA9\u1EC1\u1ED7\u1EE1\u1EF9\u1EAF\u1EB1\u1EB3\u1EB5\u1EB7\u1EB9\u1EBB\u1EBD\u1EBF\u1EC1\u1EC3\u1EC5\u1EC7\u1EC9\u1ECB\u1ECD\u1ECF\u1ED1\u1ED3\u1ED5\u1ED7\u1ED9\u1EDB\u1EDD\u1EDF\u1EE1\u1EE3\u1EE5\u1EE7\u1EE9\u1EEB\u1EED\u1EEF\u1EF1\u1EF3\u1EF5\u1EF7\u1EF9\u1EFB\u1EFD\u1EFF]");

        foreach (var c in text)
        {
            if (Regex.IsMatch(c.ToString(), "[\u4e00-\u9fa5]"))
            {
                langWeights["zh"] += 1;
                langWeights["ja"] += 1;
            }
            if (Regex.IsMatch(c.ToString(), "[\uAC00-\uD7A3]"))
            {
                langWeights["ko"] += 1;
            }
            if (vietnameseCharsRegEx.IsMatch(c.ToString()))
            {
                langWeights["vi"] += 1;
            }
            if (Regex.IsMatch(c.ToString(), "[\u0E01-\u0E5B]"))
            {
                langWeights["th"] += 1;
            }
            // if (Regex.IsMatch(c.ToString(), "[\u16F0-\u16F9]"))
            // {
            //     langWeights["hmn"] += 1;
            // }
            if (Regex.IsMatch(c.ToString(), "[\u3040-\u30ff]"))
            {
                langWeights["ja"] += 1;
            }
            if (Regex.IsMatch(c.ToString(), "[\u0400-\u04FF]"))
            {
                langWeights["ru"] += 1;
            }
            if (Regex.IsMatch(c.ToString(), "[áéíóúüñ]"))
            {
                langWeights["es"] += 1;
            }
            if (Regex.IsMatch(c.ToString(), "[àâçéèêëîïôûùüÿœæ]"))
            {
                langWeights["fr"] += 1;
            }
            if (Regex.IsMatch(c.ToString(), "[äöüß]"))
            {
                langWeights["de"] += 1;
            }
        }

        foreach (var word in text.Split(' '))
        {
            if (!Regex.IsMatch(word, "[a-zA-Z]")) continue;
            langWeights["en"] += 1;
            langWeights["es"] += 1;
            langWeights["fr"] += 1;
            langWeights["de"] += 1;
        }

        if (langWeights["zh"] > 0 && langWeights["zh"] == langWeights["ja"])
        {
            langWeights["zh"] += 1;
        }
        if (langWeights["en"] > 0 && langWeights["en"] == langWeights["es"] && langWeights["en"] == langWeights["fr"] && langWeights["en"] == langWeights["de"])
        {
            langWeights["en"] += 1;
        }

        var langWeightResult = langWeights.MaxBy(x => x.Value);

        if (langWeightResult.Value == 0)
        {
            return LangEnum.en;
        }

        return langWeightResult.Key switch
        {
            "zh" => TraditionalSimplifiedUtil.IsSimplifiedChinese(text) ? LangEnum.zh_cn : LangEnum.zh_tw,
            _ => Enum.Parse<LangEnum>(langWeightResult.Key)
        };
    }
}
using System;
using System.Collections.Generic;
using OracleSQL.Formatter.Common.Enums;

namespace OracleSQL.Formatter.UI.Localization;

public static class Localizer
{
    private static Language _current = Language.ZhTW;
    public static event Action? LanguageChanged;

    public static Language CurrentLanguage
    {
        get => _current;
        set
        {
            if (_current != value)
            {
                _current = value;
                LanguageChanged?.Invoke();
            }
        }
    }

    public static string T(string key)
    {
        var dict = _current switch
        {
            Language.EnUS => EnUS,
            Language.ZhCN => ZhCN,
            _ => ZhTW
        };
        return dict.TryGetValue(key, out var v) ? v : key;
    }

    // 繁體中文
    private static readonly Dictionary<string, string> ZhTW = new()
    {
        ["CaseOption.Unchanged"] = "不變更",
        ["CaseOption.Uppercase"] = "大寫",
        ["CaseOption.Lowercase"] = "小寫",
        ["CaseOption.InitCap"] = "字首大寫",
        ["App.Title"] = "Oracle SQL 美化器",  //別動這裡的中文字
        ["Sidebar.Functions"] = "功能",
        ["Sidebar.Beautifier"] = "美化器",
        ["Sidebar.Settings"] = "設定",
        ["Settings.Title"] = "設定",
        // string options
        ["Comma.After"] = "後置",
        ["Comma.Before"] = "前置",
        ["Comma.Before with space"] = "前置（含空格）",
        ["ListStyle.Stacked"] = "堆疊",
        ["ListStyle.Not Stacked"] = "不堆疊",
        ["Align.Align left"] = "向左對齊",
        ["Align.Align right"] = "向右對齊",
    };

    // 簡體中文
    private static readonly Dictionary<string, string> ZhCN = new()
    {
        ["CaseOption.Unchanged"] = "不变更",
        ["CaseOption.Uppercase"] = "大写",
        ["CaseOption.Lowercase"] = "小写",
        ["CaseOption.InitCap"] = "首字大写",
        ["App.Title"] = "Oracle SQL 美化器",  //別動這裡的中文字
        ["Sidebar.Functions"] = "功能",
        ["Sidebar.Beautifier"] = "美化器",
        ["Sidebar.Settings"] = "设置",
        ["Settings.Title"] = "设置",
        // string options
        ["Comma.After"] = "后置",
        ["Comma.Before"] = "前置",
        ["Comma.Before with space"] = "前置（含空格）",
        ["ListStyle.Stacked"] = "堆叠",
        ["ListStyle.Not Stacked"] = "不堆叠",
        ["Align.Align left"] = "向左对齐",
        ["Align.Align right"] = "向右对齐",
    };

    // 英文
    private static readonly Dictionary<string, string> EnUS = new()
    {
        ["CaseOption.Unchanged"] = "Unchanged",
        ["CaseOption.Uppercase"] = "Uppercase",
        ["CaseOption.Lowercase"] = "Lowercase",
        ["CaseOption.InitCap"] = "InitCap",
        ["App.Title"] = "Oracle SQL Formatter",
        ["Sidebar.Functions"] = "Functions",
        ["Sidebar.Beautifier"] = "Beautifier",
        ["Sidebar.Settings"] = "Settings",
        ["Settings.Title"] = "Settings",
        // string options
        ["Comma.After"] = "After",
        ["Comma.Before"] = "Before",
        ["Comma.Before with space"] = "Before with space",
        ["ListStyle.Stacked"] = "Stacked",
        ["ListStyle.Not Stacked"] = "Not Stacked",
        ["Align.Align left"] = "Align left",
        ["Align.Align right"] = "Align right",
    };
}

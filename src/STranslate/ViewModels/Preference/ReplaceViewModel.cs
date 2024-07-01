﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using STranslate.Helper;
using STranslate.Log;
using STranslate.Model;
using STranslate.Util;

namespace STranslate.ViewModels.Preference;

public partial class ReplaceViewModel : ObservableObject
{
    private readonly TranslatorViewModel _translateVm = Singleton<TranslatorViewModel>.Instance;
    private readonly ConfigHelper _configHelper = Singleton<ConfigHelper>.Instance;

    public ReplaceViewModel()
    {
        AllServices = _translateVm.CurTransServiceList;

        // load initial value from conf
        ReplaceProp = _configHelper.CurrentConfig?.ReplaceProp ?? new ReplaceProp();
        // View 上绑定结果从List中获取
        ReplaceProp.ActiveService = AllServices.FirstOrDefault(x => x.Identify == ReplaceProp.ActiveService?.Identify);

        _translateVm.PropertyChanged += (sender, args) =>
        {
            // 检查是否被删除
            if (sender is not BindingList<ITranslator> services || ReplaceProp.ActiveService is null) return;

            // 当服务列表中有增删时检查当前活动服务是否被删除
            if (services.All(x => x.Identify != ReplaceProp.ActiveService?.Identify))
                ReplaceProp.ActiveService = null;
        };
    }

    public async Task ExecuteAsync(string content, CancellationToken token)
    {
        if (ReplaceProp.ActiveService is null)
        {
            Singleton<NotifyIconViewModel>.Instance.ShowBalloonTip("请先选择替换翻译服务后重试");
            return;
        }
        var useLang = ReplaceProp.TargetLang;
        LogService.Logger.Debug($"<Begin> Replace Execute\tcontent: [{content}]\ttarget: [{useLang.GetDescription()}]");

        if (ReplaceProp.TargetLang == LangEnum.auto)
        {
            var identify = await LangDetectHelper.DetectAsync(content, ReplaceProp.DetectType, ReplaceProp.AutoScale, token);
            useLang = identify is LangEnum.zh_cn or LangEnum.zh_tw ? LangEnum.en : LangEnum.zh_cn;
            LogService.Logger.Debug($"ReplaceViewModel selected target lang is auto \tdetect service is [{ReplaceProp.DetectType.GetDescription()}{(ReplaceProp.DetectType == LangDetectType.Local ? $"rate is {ReplaceProp.AutoScale}" : "")}]\tdetect target lang is [{useLang.GetDescription()}]");
        }
        var req = new RequestModel(content, LangEnum.auto, useLang);
        try
        {
            if (ReplaceProp.ActiveService is ITranslatorLlm)
            {
                await ReplaceProp.ActiveService.TranslateAsync(req, InputSimulatHelper.PrintText, token);
                await EndAsync(token);
            }
            else
            {
                var ret = await ReplaceProp.ActiveService.TranslateAsync(req, CancellationToken.None);

                if (!ret.IsSuccess) throw new Exception(ret.Result?.ToString());
                InputSimulatHelper.PrintText(ret.Result);
                await EndAsync(token);
            }
        }
        catch (Exception ex)
        {
            LogService.Logger.Warn("替换翻译出错: " + ex.Message);
            return;
        }
        finally
        {
            LogService.Logger.Debug($"<End> Replace Execute");
        }

        return;

        async Task EndAsync(CancellationToken cancellationToken)
        {
            InputSimulatHelper.PrintText("✅");
            await Task.Delay(300, cancellationToken).ConfigureAwait(false);
            InputSimulatHelper.Backspace();
        }
    }

    #region Property

    [ObservableProperty] private BindingList<ITranslator> _allServices;

    [ObservableProperty] private ReplaceProp _replaceProp;
    #endregion

    #region Command

    [RelayCommand]
    private void Save()
    {
        if (_configHelper.WriteConfig(this))
        {
            ToastHelper.Show("保存替换翻译成功", WindowType.Preference);
        }
        else
        {
            LogService.Logger.Debug($"保存替换翻译失败，{JsonConvert.SerializeObject(this)}");
            ToastHelper.Show("保存替换翻译失败", WindowType.Preference);
        }
    }

    [RelayCommand]
    private void Reset()
    {
        ReplaceProp = _configHelper.CurrentConfig?.ReplaceProp ?? new ReplaceProp();

        ToastHelper.Show("重置配置", WindowType.Preference);
    }

    #endregion
}
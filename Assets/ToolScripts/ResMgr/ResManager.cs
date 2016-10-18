/**
 * Copyright (c) 2015,Need Corp. ltd;
 * All rights reserved.
 * 
 * 文件名称：ResManager.cs
 * 简    述：全局单例，资源加载管理器，提供方便的下载放法;
 *   bool GetRes(string url, OnResLoadOK callback, LoadPriority param = LoadPriority.Normal)
 *   优先级会影响资源的加载先后;
 * 创建标识：Lorry  2012/10/28
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Need.Mx
{
    public class ResManager
    {
        #region 单例;
        private ResManager() { }
        private static ResManager instance;
        public static ResManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResManager();
                }
                return instance;
            }
        }
        #endregion

        #region 私有属性;
        //private List<LoadHelper> listLoadHelper = new List<LoadHelper>();//正在开始下载的LoadHelper对象;
        //private LoadHelper loadHelper = null;
        #endregion

        #region 公有方法;
        public void LoadRes(string url, LoadHandler completeHandler)
        {
            this.LoadRes(url, completeHandler, this.ErrorHandler, this.ProgressHandler);
        }

        public void LoadXML(string url, LoadHandler completeHandler, XMLResolverHandler resolverHandler)
        {
            LoadHelper loadHelper = new LoadHelper(url, completeHandler, this.ErrorHandler, this.ProgressHandler, resolverHandler);
            IDownloader downloader = FactoryDownloader.GetDownloader(loadHelper);
            downloader.StartDown(loadHelper);
        }

        public void LoadRes(string url, LoadHandler completeHandler, LoadHandler errorHandler, LoadHandler progressHandler)
        {
            LoadHelper loadHelper = new LoadHelper(url, completeHandler, errorHandler, progressHandler);
            if (loadHelper.ExtensionName == ".xml" && loadHelper.XMLResolver == null)
            {
                Debug.LogError("加载xml文件 请使用 LoadXML方法");
            }
            IDownloader downloader = FactoryDownloader.GetDownloader(loadHelper);
            downloader.StartDown(loadHelper);
        }

        public bool CheckCache(string filePath)
        {
            return false;
        }

        public void RemoveCache(string filePath)
        {

        }

        public void CleanResCache(bool useGC = true)
        {
            FactoryDownloader.Clear(useGC);
        }

        public void ClearSceneLibrary()
        {
            FactoryDownloader.ClearSceneLibrary();
        }
        #endregion

        #region 私有方法;
        private void ErrorHandler(object obj) { }
        private void ProgressHandler(object obj) { }

        #endregion
    }
    public delegate void LoadHandler(LoadedData data);
}

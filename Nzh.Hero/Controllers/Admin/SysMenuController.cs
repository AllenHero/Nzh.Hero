﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nzh.Hero.Common.JsonExt;
using Nzh.Hero.IService;
using Nzh.Hero.Model;
using Nzh.Hero.Service;
using Nzh.Hero.ViewModel.Enum;
using Nzh.Hero.ViewModel.SystemDto;

namespace Nzh.Hero.Controllers.Admin
{
    public class SysMenuController : BaseController
    {
        private readonly ISysMenuService _menuService;

        private readonly ISysLogService _logService;

        public SysMenuController(ISysMenuService menuService, ISysLogService logService)
        {
            _menuService = menuService;
            _logService = logService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            ViewBag.Id = id;
            var menuList = _menuService.GetMenuList().Where(s => s.menu_type == 0).OrderBy(s => s.menu_level).ToList();
            menuList.Insert(0, new sys_menu() { id = 0, menu_name = "请选择" });
            ViewBag.MenuSel = new SelectList(menuList, "id", "menu_name");
            return View();
        }

        [HttpGet]
        public ActionResult GetData()
        {
            var data = _menuService.GetMenuList();
            _logService.WriteLog(LogType.VIEW, $"查询菜单", LogState.NORMAL);//写入日志
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SaveData(sys_menu dto)
        {
            if (dto.parent_id == 0)
            {
                dto.menu_url = string.Empty;
                dto.menu_icon = dto.menu_icon ?? "fa fa-desktop";
            }
            else
            {
                dto.menu_url = dto.menu_url ?? string.Empty;
                dto.menu_icon = dto.menu_icon ?? "fa fa-tag";
            }
            if (dto.id == 0)
            {
                _menuService.AddMenu(dto, string.Empty);
                _logService.WriteLog(LogType.ADD, $"添加菜单(" + dto.menu_name + ")", LogState.NORMAL);//写入日志
            }
            else
            {
                _menuService.UpdateMenu(dto, string.Empty);
                _logService.WriteLog(LogType.EDIT, $"修改菜单(" + dto.menu_name + ")", LogState.NORMAL);//写入日志
            }
            return Success("保存成功");
        }

        [HttpGet]
        public ActionResult GetDataById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Error("参数错误");
            }
            var data = _menuService.GetMenuById(id);
            var result = new ResultAdaptDto();
            result.data.Add("model", data);
            _logService.WriteLog(LogType.OTHER, $"获取菜单(" + id + ")", LogState.NORMAL);//写入日志
            return Content(result.ToJson());
        }

        [HttpGet]
        public ActionResult Del(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Error("参数错误");
            }
            _menuService.DelByIds(ids);
            _logService.WriteLog(LogType.DEL, $"删除菜单(" + ids + ")", LogState.NORMAL);//写入日志
            return Success("删除成功");
        }
    }
}
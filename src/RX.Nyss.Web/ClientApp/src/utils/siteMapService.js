import { siteMap } from "../siteMap";
import { placeholders } from "../siteMapPlaceholders";

const findClosestMenu = (breadcrumb, placeholder, pathForMenu) => {
  for (let i = breadcrumb.length - 1; i >= 0; i--) {
    if (breadcrumb[i].siteMapData.placeholder === placeholder) {
      return breadcrumb[i].siteMapData.parentPath;
    }
  }
};

const getParentLeftSidebarMenuPathFromBreadcrumb = (breadcrumb) => {
  let parentMenuPath = undefined;
  const lastCrumb = breadcrumb[breadcrumb.length - 1];
  for (let i = breadcrumb.length - 2; i >= 0; i--) {
    const previousCrumb = breadcrumb[i];
    if (previousCrumb.siteMapData.placeholder === placeholders.leftMenu) {
      // It's the second time we find a parentMenuPath: always break
      if (parentMenuPath) {
        parentMenuPath = previousCrumb.siteMapData.path;
        break;
      }
      // It's the first time we find a parentMenuPath: break if last crumb has placeholder === placeholders.leftMenu
      parentMenuPath = previousCrumb.siteMapData.path;
      if (lastCrumb.siteMapData.placeholder === placeholders.leftMenu) {
        break;
      }
    }
  }

  return parentMenuPath;
};

const getParentLeftSidebarMenuWithSubmenu = (
  pathForMenu,
  parameters,
  placeholder,
  authUser,
  breadcrumb,
  currentMenu
) => {
  const parentMenuPath = getParentLeftSidebarMenuPathFromBreadcrumb(breadcrumb);
  if (!parentMenuPath) {
    return;
  }

  const parentMenu = getMenu(
    pathForMenu,
    parameters,
    placeholder,
    parentMenuPath,
    authUser,
    true,
    true
  );
  const parent = parentMenu.find((item) => item.path === parentMenuPath);
  if (!parent) {
    return;
  }

  parent.submenu = currentMenu;
  return parentMenu;
};

export const getMenu = (
  pathForMenu,
  parameters,
  placeholder,
  currentPath,
  authUser,
  keepPath = false,
  stopRecursion = false
) => {
  const breadcrumb = getBreadcrumb(currentPath, parameters, authUser);
  const closestMenuPath = findClosestMenu(breadcrumb, placeholder, pathForMenu);
  const filteredSiteMap = siteMap.filter(
    (item) =>
      item.parentPath === closestMenuPath &&
      item.placeholder &&
      item.placeholder === placeholder &&
      item.access.some((role) => authUser.roles.some((r) => r === role)) &&
      !(!!item.hideWhen && item.hideWhen(parameters))
  );

  const currentMenu = filteredSiteMap
    .sort((a, b) => a.placeholderIndex - b.placeholderIndex)
    .map((item) => ({
      title: item.title(),
      url: getUrl(item.path, parameters),
      isActive: breadcrumb.some((b) => b.siteMapData.path === item.path),
      path: keepPath ? item.path : undefined,
    }));

  const isLeftSidebarSubmenu =
    placeholder === placeholders.leftMenu &&
    filteredSiteMap.every(
      (item) => item.isSubmenuItem && item.placeholder === placeholders.leftMenu
    );
  if (isLeftSidebarSubmenu && !stopRecursion) {
    const parentMenu = getParentLeftSidebarMenuWithSubmenu(
      pathForMenu,
      parameters,
      placeholder,
      authUser,
      breadcrumb,
      currentMenu
    );
    if (parentMenu) {
      return parentMenu;
    }
  }

  return currentMenu;
};

export const getBreadcrumb = (path, siteMapParameters, authUser) => {
  if (!authUser || !path) {
    return [];
  }

  let currentItem = findSiteMapItem(path);
  let hierarchy = [];

  while (true) {
    const hasAccess = !currentItem.access || !currentItem.access.length || currentItem.access.some(role => authUser.roles.some(r => r === role));

    if (hasAccess) {
      hierarchy.splice(0, 0, {
        title: getTitle(currentItem.title(), siteMapParameters),
        url: getUrl(currentItem.path, siteMapParameters),
        isActive: currentItem.path === path,
        siteMapData: { ...currentItem },
        hidden: hierarchy.length === 0 && currentItem.middleStepOnly
      });
    }

    if (!currentItem.parentPath) {
      break;
    }

    currentItem = findSiteMapItem(currentItem.parentPath);
  }

  return hierarchy;
}

const getTitle = (template, params) =>
  Object.keys(params).reduce((result, key) => typeof result === 'string' ? result.replace(`{${key}}`, params[key]) : result, template);

const getUrl = (template, params) =>
  Object.keys(params).reduce((result, key) => typeof result === 'string' ? result.replace(`:${key}`, params[key]) : result, template);

const findSiteMapItem = (path) => {
  const item = siteMap.find(item => item.path === path);
  if (!item) {
    throw new Error(`SiteMap configuration is inconsistent. Cannot find item with path: ${path}`)
  }
  return item;
}

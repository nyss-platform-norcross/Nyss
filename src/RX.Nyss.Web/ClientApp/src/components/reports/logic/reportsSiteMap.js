import { placeholders } from "../../../siteMapPlaceholders";
import { accessMap } from "../../../authentication/accessMap";
import { strings, stringKeys } from "../../../strings";
import { projectLeftMenuOrder } from "../../projects/logic/projectsSiteMap";

export const reportsSiteMap = [
  {
    parentPath: "/nationalsocieties/:nationalSocietyId/projects/:projectId",
    path: "/projects/:projectId/reports",
    title: () => strings(stringKeys.reports.title),
    placeholder: placeholders.leftMenu,
    access: accessMap.reports.list,
    placeholderIndex: projectLeftMenuOrder.reports,
    isSubmenuItem: true,
  },
  {
    parentPath: "/projects/:projectId/reports",
    path: "/projects/:projectId/reports/correct",
    title: () => strings(stringKeys.reports.correctReportsTitle),
    access: accessMap.reports.list,
    placeholder: placeholders.tabMenu,
    placeholderIndex: 1,
    middleStepOnly: true
  },
  {
    parentPath: "/projects/:projectId/reports",
    path: "/projects/:projectId/reports/incorrect",
    title: () => strings(stringKeys.reports.incorrectReportsTitle),
    access: accessMap.reports.list,
    placeholder: placeholders.tabMenu,
    placeholderIndex: 2,
    middleStepOnly: true
  },
  {
    parentPath: "/projects/:projectId/reports",
    path: "/projects/:projectId/reports/:reportId/edit",
    title: () => strings(stringKeys.reports.form.title),
    access: accessMap.reports.edit
  }
];

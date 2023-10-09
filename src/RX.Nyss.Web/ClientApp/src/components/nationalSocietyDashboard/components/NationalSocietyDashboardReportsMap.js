import React from "react";
import { Card, CardContent, CardHeader } from "@material-ui/core";
import { strings, stringKeys } from "../../../strings";
import { ReportsMap } from "../../maps/ReportsMap";

export const NationalSocietyDashboardReportsMap = ({
  data,
  details,
  detailsFetching,
  nationalSocietyId,
  getReportHealthRisks,
}) => {
  const handleMarkerClick = (lat, lng) =>
    getReportHealthRisks(nationalSocietyId, lat, lng);

  return (
    <Card data-printable={true}>
      <CardHeader title={strings(stringKeys.dashboard.map.title)} />
      <CardContent>
        <ReportsMap
          data={data}
          details={details}
          detailsFetching={detailsFetching}
          onMarkerClick={handleMarkerClick}
        />
      </CardContent>
    </Card>
  );
};

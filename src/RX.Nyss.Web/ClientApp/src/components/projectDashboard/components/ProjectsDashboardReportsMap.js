import styles from "./ProjectsDashboardReportsMap.module.scss";

import React, { useEffect, useState } from 'react';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import CardHeader from '@material-ui/core/CardHeader';
import MarkerClusterGroup from 'react-leaflet-markercluster';
import CircleMapMarker from '../../common/map/CircleMapMarker';
import { Map, TileLayer, Popup } from 'react-leaflet'
import { calculateBounds, calculateCenter } from '../../../utils/map';
import { ClusterIcon } from '../../common/map/ClusterIcon';
import { Loading } from '../../common/loading/Loading';
import { strings, stringKeys } from "../../../strings";

export const ProjectsDashboardReportsMap = ({ data, details, detailsFetching, projectId, getReportHealthRisks }) => {
  const [bounds, setBounds] = useState(null);
  const [center, setCenter] = useState(null);

  useEffect(() => {
    if (!data) {
      return;
    }

    setBounds(data.length > 1 ? calculateBounds(data) : null)
    setCenter(calculateCenter(data.map(l => ({ lat: l.location.latitude, lng: l.location.longitude }))));
  }, [data])

  const handleMarkerClick = e =>
    getReportHealthRisks(projectId, e.latlng.lat, e.latlng.lng);

  return (
    <Card>
      <CardHeader title={strings(stringKeys.project.dashboard.map.title)} />
      <CardContent>
        <Map
          style={{ height: "500px" }}
          zoom={5}
          bounds={bounds}
          center={center}
          scrollWheelZoom={false}
          maxZoom={25}
        >
          <TileLayer
            attribution=''
            url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          />

          <MarkerClusterGroup
            showCoverageOnHover={false}
            iconCreateFunction={cluster => ClusterIcon({ cluster })}>
            {data && data.map(point =>
              <CircleMapMarker
                key={`marker_${point.location.latitude}_${point.location.longitude}`}
                position={{ lat: point.location.latitude, lng: point.location.longitude }}
                onclick={handleMarkerClick}
              >
                <Popup>
                  <div className={styles.popup}>
                    {!detailsFetching
                      ? (
                        <div>
                          {details && details.map(h => (
                            <div className={styles.reportHealthRiskDetails} key={`reportHealthRisk_${h.name}`}>
                              <div>{h.name}:</div>
                              <div>{h.value} {strings(h.value === 1 ? stringKeys.project.dashboard.map.report : stringKeys.project.dashboard.map.reports)}</div>
                            </div>
                          ))}
                        </div>
                      )
                      : (<Loading inline noWait />)
                    }
                  </div>
                </Popup>
              </CircleMapMarker>
            )}
          </MarkerClusterGroup>
        </Map>
      </CardContent>
    </Card>
  );
}

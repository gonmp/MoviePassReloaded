import * as React from 'React';
import { RouteComponentProps } from 'react-router-dom';

export interface IPrivateRouteProps {
    reactComponente: React.FC | (({ match }: RouteComponentProps<any>) => JSX.Element)
}

const PrivateRoute = ({ reactComponente }: IPrivateRouteProps) => {

}
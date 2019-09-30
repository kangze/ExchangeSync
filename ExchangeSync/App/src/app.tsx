import * as React from "react";
import { BrowserRouter as Router, Route } from 'react-router-dom';

import Layout from "./Layout";
import Item from "./components/MailItem";
import MailDetail from "./components/MailDetail";

export default class App extends React.Component {

    public render() {
        return (
            <Layout>
                <Route path="/index" component={Item} />
                <Route path="/detail" component={MailDetail} />
            </Layout>
        );
    }
}
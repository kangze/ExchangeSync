import * as React from "react";
import { BrowserRouter as Router, Route } from 'react-router-dom';

import Layout from "./Layout";
import Item from "./components/MailItem";
import MailDetail from "./components/MailDetail";
import MailCreate from "./components/MailCreate";

export default class App extends React.Component {

    public render() {
        return (
            <Layout>
                <Route exact path="/" component={Item} />
                <Route exact path="/sended" component={Item} />
                <Route exact path="/draft" component={Item} />
                <Route path="/detail/:mailId" component={MailDetail} />
                <Route path="/create" component={MailCreate} />
                <Route path="/reply/:mailId" component={MailCreate} />
            </Layout>
        );
    }
}
import * as React from "react";


import Item from "./components/MailItem";
import Header from "./components/Header";
import MailDetail from "./components/MailDetail";
import { BrowserRouter as Router, Route } from 'react-router-dom';

export default class Layout extends React.Component {

    public render() {
        return (
            <div>
                <Route path="/" component={Header} />
                {this.props.children}
            </div>
        );
    }
}
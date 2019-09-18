import * as React from "react";

import { BrowserRouter as Router, Route } from 'react-router-dom';

import Item from "./components/MailItem";
import Header from "./components/Header";
import MailDetail from "./components/MailDetail";



export default class App extends React.Component {

    public render() {
        var s=m+10;
        return (
            <div>
                <Header />
                <Item />
                {/* <MailDetail /> */}
            </div>
        );
    }
}
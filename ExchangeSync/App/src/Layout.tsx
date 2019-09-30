import * as React from "react";


import Item from "./components/MailItem";
import Header from "./components/Header";
import MailDetail from "./components/MailDetail";

export default class Layout extends React.Component {

    public render() {
        return (
            <div>
                <Header />
                {this.props.children}
            </div>
        );
    }
}
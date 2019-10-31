import * as React from 'react';

require("./TextInput.css");

export default class TextInput extends React.Component<any, any>{
    public render() {
        return (
            <div className="textField">
                <div className="textWarpper">
                    <label className="textLable">{this.props.label}</label>
                    <div className="textFieldGroup">
                        <input multiple={this.props.multiple} onChange={this.props.onChange ? this.props.onChange : undefined} id={this.props.id} type={this.props.type} className="textGroupInput"></input>
                    </div>
                </div>
            </div>
        );
    }
}
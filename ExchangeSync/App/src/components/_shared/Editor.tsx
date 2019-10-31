import * as React from 'react';
declare var ZxEditor: any;


export default class Editor extends React.Component<any, any>{

    shouldComponentUpdate(nextProps: any, nextState: any) {
        return false;
    }

    public componentDidMount() {
        delete (window as any).zxEditor;
        var zxEditor = new ZxEditor('#e', {
            fixed: true,
            placeholder: "点击编辑..."
        });
        (window as any).zxEditor = zxEditor;
    }

    public render() {
        return (
            <div id="e">

            </div>
        );
    }
}
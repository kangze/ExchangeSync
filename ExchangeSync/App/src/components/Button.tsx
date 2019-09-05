import * as React from 'react';

interface IHomePageState {
  name: string;
}


export default class Button extends React.Component<{}, IHomePageState> {
  constructor(props: {}) {
    super(props);
    this.state = {
      name: "",
    };
  }

  public setName = () => {
    this.setState({
      name: "icepy",
    });
  }

  public render() {
    const { name } = this.state;
    return (
      <div>
        <div onClick={this.setName}> set name </div>
        <div>{name}</div>
      </div>
    )
  }
}
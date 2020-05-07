-------------控件定义--------------
-- !@#start

--!@#definestart
Img_Bg = nil;
Btn_Login = nil;
Btn_Quit = nil;

--!@#defineend

--!@#get/setstart

--!@#get/setend

-------------输入监听--------------
function LoginUI:InitBtnListener()

end

-- !@#startClick

-- !@#endClick

function LoginUI:InitFEventListener()

end

-- !@#startFEvent

-- !@#endFEvent

-- !@#end

------------生命周期------------------
function LoginUI:OnLoadSuccess()
	self:InitBtnListener();
	self:InitFEventListener();
end

function LoginUI:OnInit()
  
end

function LoginUI:OnClickMaskArea()
  
end

function LoginUI:Dispose()
  
end

function LoginUI:OnClose()
  
end
